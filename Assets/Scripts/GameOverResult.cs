using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverResult : MonoBehaviour
{
    [SerializeField] private ScoreModel scoreModel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RankingView rankingView;
    private IRanking ranking;
    private string databaseName = "local_ranking.db";
    private string checkUrl = "http://192.168.210.105/";
    [SerializeField] private int timeOutSeconds = 2;
    
    void Start()
    {
        AudioManager.Instance.PlaySE("end");
        scoreText.text = $"スコア：{scoreModel.ScorePoint.CurrentValue}<size=24>コ";
        Ranking().Forget();
    }

    private async UniTask<bool> CheckConnectionAsync(CancellationToken ct)
    {
        using var request = new UnityWebRequest(checkUrl) { timeout = timeOutSeconds };
        var operation = request.SendWebRequest();
        await UniTask.WaitUntil(() => operation.isDone, cancellationToken: ct);
        // 404NotFoundだとProtocolErrorが出るので許す
        return request.result == UnityWebRequest.Result.Success || request.result == UnityWebRequest.Result.ProtocolError;
    }
    
    async UniTaskVoid Ranking()
    {
        bool canConnect = await CheckConnectionAsync(this.GetCancellationTokenOnDestroy());
        //Debug.Log("canConnect : "+canConnect);
        if (canConnect)
        {
            ranking = new ServerRanking();
        }
        else
        {
            ranking = new InMemoryRanking(System.IO.Path.Combine(Application.persistentDataPath, databaseName));
        }
        
        Record record = new Record(scoreModel.ScorePoint.CurrentValue);
        bool isRankedIn = await ranking.IsRankedInAsync(record);
        if (isRankedIn)
        {
            List<Record> records = await ranking.GetRankingAsync();
            records.Add(record);
            Record[] newRecords = records.OrderByDescending(x => x.Score).Take(5).ToArray();
            rankingView.ShowRanking(newRecords);
            int newRecordIndex = Array.IndexOf(newRecords, record);
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
