using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Camera myCamera;
    [SerializeField] private Image ink;
    [SerializeField] private Animator damageEffect;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float force = 1f;
    static readonly int DamageHash = Animator.StringToHash("Damage");
    private readonly float[] _hitScale = new float[] { 1, 1.5f, 2f };

    public void PlayEffect()
    {
        AudioManager.Instance.PlaySE("damage");
        damageEffect.Play(DamageHash);
        impulseSource.GenerateImpulse();
    }

    public void HitDecal(int hp, Vector3 position)
    {
        Vector3 screenPoint = myCamera.WorldToScreenPoint(position);
        Image obj = Instantiate(ink, screenPoint, Quaternion.identity, parent);
        int hitIndex = 2 - hp;
        obj.transform.localScale = Vector3.one * _hitScale[hitIndex];
    }
}
