using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ParabolaDemoObjectMover : MonoBehaviour, IObjectMovable
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private Vector3 startPosition = Vector3.zero;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float power = 1f;
    
    public void ChangeSpawnPosition(Transform target)
    {
        Vector3 position = targetPosition + Random.onUnitSphere * distance;
        target.position = new Vector3(position.x, startPosition.y, targetPosition.z);
        Vector3 direction = (targetPosition - target.position).normalized;
        Rigidbody rb = target.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.ResetInertiaTensor();
        rb.AddForce(direction * power, ForceMode.Impulse);
    }

    public void UpdateMovePosition(Transform target)
    {
        
    }
}
