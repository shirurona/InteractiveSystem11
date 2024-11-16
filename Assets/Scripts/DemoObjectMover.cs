using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DemoObjectMover : MonoBehaviour, IObjectMovable
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float speed = 1f;
    private Vector3 _direction;
    
    public void ChangeSpawnPosition(Transform target)
    {
        Vector3 position = targetPosition + Random.onUnitSphere * distance;
        target.position = position;
        _direction = (targetPosition - target.position).normalized;
    }

    public void UpdateMovePosition(Transform target)
    {
        target.position += _direction * (Time.deltaTime * speed);
    }
}
