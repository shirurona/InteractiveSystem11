using System;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private Behaviour objectMovable;
    private IObjectMovable _objectMovable;

    private void Awake()
    {
        _objectMovable = objectMovable.GetComponent<IObjectMovable>();
    }

    private void OnEnable()
    {
        _objectMovable.ChangeSpawnPosition(transform);
    }

    // Update is called once per frame
    void Update()
    {
        _objectMovable.UpdateMovePosition(transform);
    }
}
