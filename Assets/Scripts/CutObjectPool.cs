using System;
using UnityEngine;
using UnityEngine.Pool;

public class CutObjectPool : MonoBehaviour
{
    public IObjectPool<GameObject> CutPool
    {
        get
        {
            _cutPool ??= new ObjectPool<GameObject>(CreateCuttedItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, maxPoolSize);
            return _cutPool;
        }
    }
    private IObjectPool<GameObject> _cutPool;
    
    [SerializeField] private GameObject cuttedObject;
    public int maxPoolSize = 10;

    GameObject CreateCuttedItem()
    {
        GameObject go = Instantiate(cuttedObject, transform);
        go.GetComponent<TimeReleaser>().ReturnPool = CutPool;
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
