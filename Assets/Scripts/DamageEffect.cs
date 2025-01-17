using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Camera myCamera;
    [SerializeField] private Image ink;
    [SerializeField] private Animator damageEffect;
    [SerializeField] private Animator gameOverEffect;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float force = 1f;
    
    static readonly int GameOverHash = Animator.StringToHash("GameOver");
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
    
    public async UniTaskVoid OnGameOver(CancellationToken cts)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("Result");
        loadScene.allowSceneActivation = false;
        AudioManager.Instance.StopBGM();
        gameOverEffect.Play(GameOverHash);
        await UniTask.WaitForSeconds(0.5f, cancellationToken:cts);
        loadScene.allowSceneActivation = true;
    }
}
