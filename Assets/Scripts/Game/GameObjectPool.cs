using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool : MonoBehaviour
{
    public IObjectPool<GameObject> ObjectPool
    {
        get
        {
            _pool ??= new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, maxPoolSize);
            return _pool;
        }
    }
    private IObjectPool<GameObject> _pool;
    private IObjectMovable _objectMovable;
    [SerializeField] private CutObjectPool cutObjectPool;
    [SerializeField] private GameObject spawnObject;
    public Observable<Vector3> OnHit => _onHit;
    private Subject<Vector3> _onHit = new Subject<Vector3>();
    public Observable<Unit> OnCut => _onCut;
    private Subject<Unit> _onCut = new Subject<Unit>();
    public int maxPoolSize = 10;
    
    GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(spawnObject, transform);
        ObjectEffect effect = go.GetComponentInChildren<ObjectEffect>();
        effect.ObjectPool = ObjectPool;
        effect.CutPool = cutObjectPool.CutPool;
        TimeReleaser releaser = go.GetComponent<TimeReleaser>();
        releaser.ReturnPool = ObjectPool;
        return go;
    }

    void InitObject(GameObject go)
    {
        AudioManager.Instance.PlaySE("appear");
        var dct = go.GetCancellationTokenOnDestroy();
        ObjectCancel cancel = go.GetComponent<ObjectCancel>();
        var cts = CancellationTokenSource.CreateLinkedTokenSource(dct, cancel.GetToken());
        ObjectEffect effect = go.GetComponentInChildren<ObjectEffect>();
        InitObjectEffect(effect, cts.Token);
        TimeReleaser releaser = go.GetComponent<TimeReleaser>();
        InitTimeReleaser(releaser, cts.Token);
    }

    void InitObjectEffect(ObjectEffect effect, CancellationToken cts)
    {
        UniTask.Void(async () =>
        {
            await effect.OnObjectCutAsync(cts);
            _onCut.OnNext(Unit.Default);
        });
    }

    void InitTimeReleaser(TimeReleaser releaser, CancellationToken cts)
    {
        UniTask.Void(async () =>
        {
            await releaser.OnReleaseAsync(cts);
            _onHit.OnNext(releaser.transform.position);
        });
    }
    
    void OnReturnedToPool(GameObject go)
    {
        go.GetComponent<ObjectCancel>().TryCancel();
        go.SetActive(false);
    }
    
    void OnTakeFromPool(GameObject go)
    {
        InitObject(go);
        go.SetActive(true);
    }
    
    void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }
}
