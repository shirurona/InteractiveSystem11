using System;
using R3;
using UnityEngine;

public class HealthModel : MonoBehaviour
{
    [SerializeField] private int maxHp = 3;
    public ReadOnlyReactiveProperty<int> HitPoint => Hp;

    private ReactiveProperty<int> Hp
    {
        get
        {
            _hp ??= new ReactiveProperty<int>(maxHp);
            return _hp;
        }
    }
    private ReactiveProperty<int> _hp;
    public void OnHitDamage()
    {
        Hp.Value = Mathf.Max(Hp.CurrentValue - 1, 0);
    }
}
