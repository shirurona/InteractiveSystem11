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
    [SerializeField] private HealthModel health;
    [SerializeField] private ScoreModel score;
    public int maxPoolSize = 10;
    
    GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(spawnObject, transform);
        ObjectEffect effect = go.GetComponentInChildren<ObjectEffect>();
        InitObjectEffect(effect);
        TimeReleaser releaser = go.GetComponent<TimeReleaser>();
        InitTimeReleaser(releaser);
        return go;
    }

    void InitObjectEffect(ObjectEffect effect)
    {
        effect.ObjectPool = ObjectPool;
        effect.CutPool = cutObjectPool.CutPool;
        var ct = this.GetCancellationTokenOnDestroy();
        UniTask.Void(async () =>
        {
            await effect.OnObjectCutAsync(ct);
            score.OnCut();
        });
    }

    void InitTimeReleaser(TimeReleaser releaser)
    {
        releaser.ReturnPool = ObjectPool;
        var ct = this.GetCancellationTokenOnDestroy();
        UniTask.Void(async () =>
        {
            await releaser.OnReleaseAsync(ct);
            health.OnHitDamage();
        });
    }
    
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
    }
    
    void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }
    
    void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }
}
