using UnityEngine;

public class DebuggerMouse : MonoBehaviour
{
    [SerializeField] private Camera myCamera;
    [SerializeField] private int triggerCount = 10;
    private Transform _myTransform;
    private int _count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _count = 0;
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 右クリック10回で操作できるようになる
        if (Input.GetMouseButtonDown(1))
        {
            _count++;
        }
        if (_count < triggerCount) return;

        _myTransform.position = myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.5f));
    }
}
