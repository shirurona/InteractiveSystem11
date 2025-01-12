using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObjectPool pool;
    [SerializeField] private HealthModel model;
    [SerializeField] private float[] intervalTime;

    private float _time = 0f;
    private int _count = 0;

    private void Start()
    {
        AudioManager.Instance.PlayBGM("bgm", 0.2f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        int intervalIndex = Mathf.Min(_count, intervalTime.Length - 1);
        if (_time > intervalTime[intervalIndex])
        {
            _time -= intervalTime[intervalIndex];
            pool.ObjectPool.Get();
            _count++;
        }
        _time += Time.deltaTime;
    }
}
