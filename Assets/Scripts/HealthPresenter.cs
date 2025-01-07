using R3;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private HealthModel model;
    [SerializeField] private HealthView view;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model.HitPoint
            .Subscribe(view.ShowHearts)
            .AddTo(this);
    }
}
