using Unity.Cinemachine;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private Animator damageEffect;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float force = 1f;
    static readonly int DamageHash = Animator.StringToHash("Damage");

    public void PlayEffect()
    {
        damageEffect.Play(DamageHash);
        impulseSource.GenerateImpulse();
    }
}
