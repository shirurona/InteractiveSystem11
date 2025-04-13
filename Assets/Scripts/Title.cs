using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject overlayCanvas;
    [SerializeField] private GameObject startButton;
    [SerializeField] private RankingView rankingView;
    [SerializeField] private RankingFactory rankingFactory;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator transitionAnimator;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    
    [SerializeField] private float rankingViewSeconds = 10;
    
    private static readonly int TextFadeAnimationHash = Animator.StringToHash("TextFade"); 
    private static readonly int TextReturnAnimationHash = Animator.StringToHash("TextReturn");
    private static readonly int BlackOutAnimationHash = Animator.StringToHash("BlackOut");
    
    private bool _isRankingButtonPressed = false;
    private bool _isRankingLoop = false;
    private bool _once = false;
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM("title");
        RankingLoop(this.GetCancellationTokenOnDestroy()).Forget();
        Observable.EveryValueChanged(leftHand, x => x.activeSelf)
            .Where(x => x)
            .Where(_ => !_isRankingButtonPressed && overlayCanvas.activeSelf)
            .Subscribe(_ => TitleScene())
            .AddTo(this);
        Observable.EveryValueChanged(rightHand, x => x.activeSelf)
            .Where(x => x)
            .Where(_ => !_isRankingButtonPressed && overlayCanvas.activeSelf)
            .Subscribe(_ => TitleScene())
            .AddTo(this);
        
        Observable.EveryValueChanged(leftHand, x => x.activeSelf)
            .Where(x => !x)
            .Where(_ => !rightHand.activeSelf)
            .Subscribe(_ => RankingLoop(this.GetCancellationTokenOnDestroy()).Forget())
            .AddTo(this);
        Observable.EveryValueChanged(rightHand, x => x.activeSelf)
            .Where(x => !x)
            .Where(_ => !leftHand.activeSelf)
            .Subscribe(_ => RankingLoop(this.GetCancellationTokenOnDestroy()).Forget())
            .AddTo(this);
    }
    
    private async UniTaskVoid RankingLoop(CancellationToken ct)
    {
        if (_isRankingLoop) return;
        _isRankingLoop = true;
        while (!leftHand.activeSelf && !rightHand.activeSelf)
        {
            TitleScene();
            await UniTask.WaitForSeconds(rankingViewSeconds, cancellationToken: ct);
            RankingScene(this.GetCancellationTokenOnDestroy()).Forget();
            await UniTask.WaitForSeconds(rankingViewSeconds, cancellationToken: ct);
        }
        _isRankingLoop = false;
    }

    public void OnRankingButton()
    {
        if (_isRankingButtonPressed || overlayCanvas.activeSelf)
        {
            TitleScene();
        }
        else
        {
            RankingScene(this.GetCancellationTokenOnDestroy()).Forget();
        }
        _isRankingButtonPressed = !_isRankingButtonPressed;
    }

    private async UniTaskVoid RankingScene(CancellationToken ct)
    {
        animator.Play(TextFadeAnimationHash);
        startButton.SetActive(false);
        await UniTask.WaitForSeconds(1, cancellationToken: ct);
        overlayCanvas.SetActive(true);
        IRanking ranking = await rankingFactory.CreateRanking(this.GetCancellationTokenOnDestroy());
        List<Record> records = await ranking.GetRankingAsync();
        rankingView.ShowRanking(records.ToArray());
    }

    private void TitleScene()
    {
        overlayCanvas.SetActive(false);
        startButton.SetActive(true);
        animator.Play(TextReturnAnimationHash);
    }

    public void Game()
    {
        if (_once)
        {
            return;
        }
        _once = true;
        
        AudioManager.Instance.PlaySE("decide");
        transitionAnimator.Play(BlackOutAnimationHash);
        UniTask.Void(async () =>
        {
            await UniTask.WaitForSeconds(1);
            SceneManager.LoadScene("Game");
        });
    }
}
