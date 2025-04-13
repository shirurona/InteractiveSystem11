using UnityEngine;

public class CircleDemoObjectMover : MonoBehaviour, IObjectMovable
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float speed = 1f;
    private Vector3 _direction;
    
    public void ChangeSpawnPosition(Transform target)
    {
        Vector3 position = targetPosition + Random.onUnitSphere * distance;
        target.position = new Vector3(position.x, position.y, targetPosition.z);
        _direction = (targetPosition - target.position).normalized;
    }

    public void UpdateMovePosition(Transform target)
    {
        target.position += _direction * (Time.deltaTime * speed);
    }
}
