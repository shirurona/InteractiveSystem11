using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LocalGravity : MonoBehaviour {
    public Vector3 localGravity;
    private Rigidbody _rb;

    void Start () {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    void FixedUpdate () {
        _rb.AddForce(localGravity, ForceMode.Acceleration);
    }
}