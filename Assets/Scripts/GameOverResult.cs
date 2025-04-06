using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverResult : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private ScoreModel scoreModel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RankingView rankingView;
    [SerializeField] private RankingFactory rankingFactory;
    [SerializeField] private GameObject hands; 
    
    private static readonly int BlackOutAnimationHash = Animator.StringToHash("BlackOut");
    
    void Start()
    {
        AudioManager.Instance.PlaySE("end");
        scoreText.text = $"スコア：{scoreModel.ScorePoint.CurrentValue}<size=24>コ";
        Ranking().Forget();
    }
    
    async UniTaskVoid Ranking()
    {
        IRanking ranking = await rankingFactory.CreateRanking(this.GetCancellationTokenOnDestroy());
        
        Record record = new Record(scoreModel.ScorePoint.CurrentValue);
        bool isRankedIn = await ranking.IsRankedInAsync(record);
        if (isRankedIn)
        {
            List<Record> records = await ranking.GetRankingAsync();
            records.Add(record);
            foreach (var rec in records)
            {
                Debug.Log(rec.Score+" : "+rec.id);
            }
            Record[] newRecords = records.OrderByDescending(x => x.Score).ThenByDescending(x => x.id).Take(5).ToArray();
            rankingView.ShowRanking(newRecords);
            int newRecordIndex = Array.IndexOf(newRecords, record);
            Debug.Log(newRecordIndex);
            rankingView.SetInputFieldPosition(newRecordIndex);
            string decideName = await rankingView.NameRegisterAsync();
            record = new Record(scoreModel.ScorePoint.CurrentValue, decideName);
        }
        ranking.SendScoreAsync(record);
        hands.SetActive(true);
    }
    
    public void Retry()
    {
        AudioManager.Instance.PlaySE("decide");
        transitionAnimator.Play(BlackOutAnimationHash);
        UniTask.Void(async () =>
        {
            await UniTask.WaitForSeconds(1);
            SceneManager.LoadScene("Demo");
        });
    }

    public void Title()
    {
        AudioManager.Instance.PlaySE("decide");
        transitionAnimator.Play(BlackOutAnimationHash);
        UniTask.Void(async () =>
        {
            await UniTask.WaitForSeconds(1);
            SceneManager.LoadScene("Title");
        });
    }
}
