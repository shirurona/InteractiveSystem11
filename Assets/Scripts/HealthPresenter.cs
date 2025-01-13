using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private HealthModel model;
    [SerializeField] private HealthView view;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private DamageEffect damageEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model.HitPoint
            .Skip(1)
            .Subscribe(x =>
            {
                view.ShowHearts(x);
                damageEffect.PlayEffect();
                if (x == 0)
                {
                    gameOver.OnGameOver(this.GetCancellationTokenOnDestroy()).Forget();
                }
            })
            .AddTo(this);
    }
}
