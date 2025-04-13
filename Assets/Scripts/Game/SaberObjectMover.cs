using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SaberObjectMover : MonoBehaviour, IObjectMovable
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private Vector2 spawnRange = new Vector2(5f, 5f);
    [SerializeField] private Vector2 spawnPositionCenter = Vector2.zero;
    [SerializeField] private Vector2 targetRange = new Vector2(5f, 5f);
    [SerializeField] private Vector3 targetPositionCenter = Vector3.zero;
    [SerializeField] private float arriveTime = 1f;
    private Vector3 _direction;
    
    public void ChangeSpawnPosition(Transform target)
    {
        Vector3 spawnPosition = new Vector3(spawnPositionCenter.x, spawnPositionCenter.y, 0) + new Vector3((Random.value - 0.5f) * spawnRange.x, (Random.value - 0.5f) * spawnRange.y, targetPositionCenter.z + distance);
        target.position = spawnPosition;
        Vector3 targetPosition = targetPositionCenter + new Vector3((Random.value - 0.5f) * targetRange.x, (Random.value - 0.5f) * targetRange.y, 0);
        _direction = (targetPosition - target.position).normalized;
    }

    public void UpdateMovePosition(Transform target)
    {
        float speed = distance / arriveTime;
        target.position += _direction * (Time.deltaTime * speed);
    }
}
