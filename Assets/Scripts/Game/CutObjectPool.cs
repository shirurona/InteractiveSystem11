using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        TimeReleaser timeReleaser = go.GetComponent<TimeReleaser>();
        timeReleaser.ReturnPool = CutPool;
        return go;
    }
    
    void InitTimeReleaser(TimeReleaser releaser)
    {
        var dct = releaser.GetCancellationTokenOnDestroy();
        releaser.OnReleaseAsync(dct).Forget();
    }
    
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
    }

    void OnTakeFromPool(GameObject go)
    {
        TimeReleaser timeReleaser = go.GetComponent<TimeReleaser>();
        InitTimeReleaser(timeReleaser);
        go.SetActive(true);
    }
    
    void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }
}
