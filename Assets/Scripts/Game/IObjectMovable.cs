using UnityEngine;

public interface IObjectMovable
{
    public void ChangeSpawnPosition(Transform target);
    public void UpdateMovePosition(Transform target);
}
