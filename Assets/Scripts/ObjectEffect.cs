using System;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectEffect : MonoBehaviour
{
    public IObjectPool<GameObject> ObjectPool { get; set; }
    public IObjectPool<GameObject> CutPool { get; set; }
    public float divideDistance = 0.001f;
    private bool once = false;

    private void OnEnable()
    {
        once = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (once) return;
        once = true;
        
        AppearLeftObject();
        AppearRightObject();
        ObjectPool.Release(transform.parent.gameObject);
    }

    private void AppearLeftObject()
    {
        Transform left = CutPool.Get().transform;
        left.position = transform.parent.position + Vector3.left * divideDistance;
        left.rotation = Quaternion.Euler(0,180,0);
    }
    
    private void AppearRightObject()
    {
        Transform right = CutPool.Get().transform;
        right.position = transform.parent.position + Vector3.right * divideDistance;
    }
}
