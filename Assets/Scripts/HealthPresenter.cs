using R3;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private HealthModel model;
    [SerializeField] private HealthView view;
    [SerializeField] private GameOver gameOver;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model.HitPoint
            .Skip(1)
            .Subscribe(x =>
            {
                view.ShowHearts(x);
                view.DamageEffect();
                if (x == 0)
                {
                    gameOver.OnGameOver().Forget();
                }
            })
            .AddTo(this);
    }
}
