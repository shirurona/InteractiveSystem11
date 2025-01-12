using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectEffect : MonoBehaviour
{
    public IObjectPool<GameObject> ObjectPool { get; set; }
    public IObjectPool<GameObject> CutPool { get; set; }
    public float divideDistance = 0.001f;
    public float dividePower = 0.01f;
    
    public async UniTask OnObjectCutAsync(CancellationToken ct)
    {
        await this.GetAsyncTriggerEnterTrigger().OnTriggerEnterAsync(ct);
        AudioManager.Instance.PlaySE("cut");
        AppearLeftObject();
        AppearRightObject();
        ObjectPool.Release(transform.parent.gameObject);
    }

    private void AppearLeftObject()
    {
        Transform left = CutPool.Get().transform;
        left.position = transform.parent.position + Vector3.left * divideDistance;
        left.rotation = Quaternion.Euler(0,180,0);
        Rigidbody rb = left.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.left * dividePower, ForceMode.Impulse);
    }
    
    private void AppearRightObject()
    {
        Transform right = CutPool.Get().transform;
        right.position = transform.parent.position + Vector3.right * divideDistance;
        Rigidbody rb = right.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.right * dividePower, ForceMode.Impulse);
    }
}
