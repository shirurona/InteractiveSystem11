using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

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
    [SerializeField] private HealthModel health;
    [SerializeField] private ScoreModel score;

    [SerializeField] private Transform parent;
    [SerializeField] private Camera camera;
    [SerializeField] private Image ink;
    public int maxPoolSize = 10;
    private readonly float[] _hitScale = new float[] { 1, 1.5f, 2f };
    
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
            score.OnCut();
        });
    }

    void InitTimeReleaser(TimeReleaser releaser, CancellationToken cts)
    {
        UniTask.Void(async () =>
        {
            await releaser.OnReleaseAsync(cts);
            AudioManager.Instance.PlaySE("damage");
            health.OnHitDamage();
            Vector3 screenPoint = camera.WorldToScreenPoint(releaser.transform.position);
            Image obj = Instantiate(ink, screenPoint, Quaternion.identity, parent);
            int hitIndex = 2 - health.HitPoint.CurrentValue;
            obj.transform.localScale = Vector3.one * _hitScale[hitIndex];
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
