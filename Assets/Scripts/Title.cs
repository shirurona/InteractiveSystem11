using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject overlayCanvas;
    [SerializeField] private RankingView rankingView;
    [SerializeField] private RankingFactory rankingFactory;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator transitioAnimator;
    [SerializeField] private float rankingViewSeconds = 10;
    
    private static readonly int TextFadeAnimationHash = Animator.StringToHash("TextFade"); 
    private static readonly int TextReturnAnimationHash = Animator.StringToHash("TextReturn");
    private static readonly int BlackOutAnimationHash = Animator.StringToHash("BlackOut");
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM("title");
        Observable.Timer(TimeSpan.FromSeconds(rankingViewSeconds), TimeSpan.FromSeconds(rankingViewSeconds))
            .Subscribe(_ => SceneSwitch())
            .AddTo(this);
    }

    private void SceneSwitch()
    {
        if (overlayCanvas.activeSelf)
        {
            TitleScene();
        }
        else
        {
            RankingScene().Forget();
        }
    }

    private async UniTaskVoid RankingScene()
    {
        Debug.Log("ranking scene");
        animator.Play(TextFadeAnimationHash);
        await UniTask.WaitForSeconds(1);
        overlayCanvas.SetActive(true);
        IRanking ranking = await rankingFactory.CreateRanking(this.GetCancellationTokenOnDestroy());
        List<Record> records = await ranking.GetRankingAsync();
        rankingView.ShowRanking(records.ToArray());
    }

    private void TitleScene()
    {
        Debug.Log("title scene");
        overlayCanvas.SetActive(false);
        animator.Play(TextReturnAnimationHash);
    }

    public void Game()
    {
        AudioManager.Instance.PlaySE("decide");
        transitioAnimator.Play(BlackOutAnimationHash);
        UniTask.Void(async () =>
        {
            await UniTask.WaitForSeconds(1);
            SceneManager.LoadScene("Demo");
        });
    }
}
