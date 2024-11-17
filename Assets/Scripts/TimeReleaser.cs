using System;
using UnityEngine;
using UnityEngine.Pool;

public class TimeReleaser : MonoBehaviour
{
    [SerializeField] private float deleteTime = 2f;
    public IObjectPool<GameObject> ReturnPool { get; set; }
    private bool once = false;
    private float time = 0f;

    private void OnEnable()
    {
        once = false;
        time = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (once) return;
        if (time > deleteTime)
        {
            once = true;
            ReturnPool.Release(gameObject);
        }
        time += Time.deltaTime;
    }
}
