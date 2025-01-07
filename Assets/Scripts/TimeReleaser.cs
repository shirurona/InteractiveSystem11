using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class TimeReleaser : MonoBehaviour
{
    [SerializeField] private float deleteTime = 2f;
    public IObjectPool<GameObject> ReturnPool { get; set; }
    public async UniTask OnReleaseAsync(CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(deleteTime), cancellationToken: ct);
        ReturnPool.Release(gameObject);
    }
}
