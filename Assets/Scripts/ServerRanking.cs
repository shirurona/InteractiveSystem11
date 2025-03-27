using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ServerRanking : IRanking
{
    private static readonly string RankingServerUrl = "http://192.168.210.105/api/";

    public async UniTask SendScoreAsync(Record record)
    {
        await HttpService.PostAsync(
            Path.Combine(RankingServerUrl, "send_score"),
            new Dictionary<string, string>()
            {
                { "id", record.id },
                { "name", record.Name },
                { "score", record.Score.ToString() }
            }
        );
    }

    public async UniTask<List<Record>> GetRankingAsync()
    {
        var response = await HttpService.GetAsync<ResponseRanking>(Path.Combine(RankingServerUrl, "get_ranking"));
        return response.Records;
    }

    public async UniTask<bool> IsRankedInAsync(Record record)
    {
        Debug.Log(Path.Combine(RankingServerUrl, "is_rankedin"));
        var response = await HttpService.GetAsync<ResponseRankedIn>(
            Path.Combine(RankingServerUrl, "is_rankedin"),
            new Dictionary<string, string>()
            {
                { "score", record.Score.ToString() }
            }
        );
        return response.Rankin;
    }
}
