using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private HealthModel model;
    [SerializeField] private HealthView view;
    [SerializeField] private DamageEffect damageEffect;
    [SerializeField] private GameObjectPool objectHitModel;

    [SerializeField] private float noDamageSeconds = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectHitModel.OnHit
            .ThrottleFirst(TimeSpan.FromSeconds(noDamageSeconds))
            .Subscribe(_ => model.OnHitDamage())
            .AddTo(this);
        
        Observable.ZipLatest<int, Vector3, (int, Vector3)>(model.HitPoint, objectHitModel.OnHit, (x, y) => (x, y))
            .Subscribe(x =>
            {
                view.ShowHearts(x.Item1);
                damageEffect.PlayEffect();
                damageEffect.HitDecal(x.Item1, x.Item2);
                if (x.Item1 == 0)
                {
                    damageEffect.OnGameOver(this.GetCancellationTokenOnDestroy()).Forget();
                }
            });
    }
}
