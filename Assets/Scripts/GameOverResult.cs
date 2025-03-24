using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverResult : MonoBehaviour
{
    [SerializeField] private ScoreModel scoreModel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RankingView rankingView;
    private IRanking ranking;
    private string databaseName = "local_ranking.db";
    
    void Start()
    {
        AudioManager.Instance.PlaySE("end");
        scoreText.text = $"スコア：{scoreModel.ScorePoint.CurrentValue}<size=24>コ";
        //*
        ranking = new InMemoryRanking(System.IO.Path.Combine(Application.persistentDataPath, databaseName));
        /*/
        ranking = new ServerRanking();
        //*/
        Ranking().Forget();
    }
    
    async UniTaskVoid Ranking()
    {
        Record record = new Record(scoreModel.ScorePoint.CurrentValue);
        bool isRankedIn = await ranking.IsRankedInAsync(record);
        if (isRankedIn)
        {
            List<Record> records = await ranking.GetRankingAsync();
            records.Add(record);
            Record[] newRecords = records.OrderByDescending(x => x.Score).Take(5).ToArray();
            rankingView.ShowRanking(newRecords);
            int newRecordIndex = System.Array.IndexOf(newRecords, record);
            rankingView.SetInputFieldPosition(newRecordIndex);
            string decideName = await rankingView.NameRegisterAsync();
            record = new Record(scoreModel.ScorePoint.CurrentValue, decideName);
        }
        ranking.SendScoreAsync(record);
    }
    
    public void Retry()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Demo");
    }

    public void Title()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Title");
    }
}
