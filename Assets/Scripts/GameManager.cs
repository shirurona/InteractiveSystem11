using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObjectPool pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            GameObject obj = pool.Pool.Get();
            yield return new WaitForSeconds(1);
            pool.Pool.Release(obj);
        }
    }
}
