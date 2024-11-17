using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObjectPool pool;

    [SerializeField] private float intervalTime = 1f;
    [SerializeField] private float deleteTime = 2f;

    private float time = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (time > intervalTime)
        {
            time -= intervalTime;
            pool.ObjectPool.Get();
        }
        time += Time.deltaTime;
    }
}
