using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool : MonoBehaviour
{
    public IObjectPool<GameObject> Pool
    {
        get
        {
            _pool ??= new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, maxPoolSize);
            return _pool;
        }
    }
    private IObjectPool<GameObject> _pool;
    private IObjectMovable _objectMovable;
    [SerializeField] private GameObject spawnObject;
    public int maxPoolSize = 10;
    
    GameObject CreatePooledItem()
    {
        return Instantiate(spawnObject, transform);
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
