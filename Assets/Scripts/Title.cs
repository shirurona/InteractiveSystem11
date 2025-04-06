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
    [SerializeField] private float rankingViewSeconds = 10;
    
    private static readonly int TextFadeAnimationHash = Animator.StringToHash("TextFade"); 
    private static readonly int TextReturnAnimationHash = Animator.StringToHash("TextReturn");
    private static readonly int BlackOutAnimationHash = Animator.StringToHash("BlackOut");

    private IDisposable _rankingShowDisposable;
    private IDisposable _rankingCloseDisposable;
    private bool _isRankingButtonPressed = false;
    private bool _once = false;
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM("title");
        _rankingShowDisposable = Observable.Timer(TimeSpan.FromSeconds(rankingViewSeconds))
            .Subscribe(_ =>
            {
                _rankingCloseDisposable = Observable.Timer(TimeSpan.FromSeconds(rankingViewSeconds))
                    .Subscribe(_ => TitleScene());
                RankingScene(this.GetCancellationTokenOnDestroy()).Forget();
            });
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
        _rankingShowDisposable?.Dispose();
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
        _rankingCloseDisposable?.Dispose();
        overlayCanvas.SetActive(false);
        startButton.SetActive(true);
        animator.Play(TextReturnAnimationHash);
        _rankingShowDisposable = Observable.Timer(TimeSpan.FromSeconds(rankingViewSeconds))
            .Subscribe(_ =>
            {
                _rankingCloseDisposable = Observable.Timer(TimeSpan.FromSeconds(rankingViewSeconds))
                    .Subscribe(_ => TitleScene());
                RankingScene(this.GetCancellationTokenOnDestroy()).Forget();
            });
    }

    public void Game()
    {
        if (_once)
        {
            return;
        }
        _once = true;
        
        AudioManager.Instance.PlaySE("decide");
        _rankingShowDisposable?.Dispose();
        _rankingCloseDisposable?.Dispose();
        transitionAnimator.Play(BlackOutAnimationHash);
        UniTask.Void(async () =>
        {
            await UniTask.WaitForSeconds(1);
            SceneManager.LoadScene("Demo");
        });
    }
}
