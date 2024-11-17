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
