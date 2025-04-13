using R3;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PhysicalHandsButtonLongPressView : MonoBehaviour
{
    [SerializeField] private PhysicalHandsButtonCustom physicalHandsButton;
    private Renderer _renderer;

    private readonly int _sliderValueKeyword = Shader.PropertyToID("_SliderValue");
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        physicalHandsButton.LongPressProperty
            .Subscribe(x => _renderer.material.SetFloat(_sliderValueKeyword, x * 2 * Mathf.PI))
            .AddTo(this);
    }
}
