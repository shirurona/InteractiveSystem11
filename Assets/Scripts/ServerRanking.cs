using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ServerRanking : IRanking
{
    private static readonly string RankingServerUrl = "http://192.168.210.7/api/";

    public async UniTask SendScoreAsync(Record record)
    {
        await HttpService.Post(
            RankingServerUrl,
            new Dictionary<string, string>()
            {
                { "name", record.Name },
                { "score", record.Score.ToString() }
            }
        );
    }

    public async UniTask<List<Record>> GetRankingAsync()
    {
        return await HttpService.Get<List<Record>>(RankingServerUrl);
    }

    public async UniTask<bool> IsRankedInAsync(Record record)
    {
        return await HttpService.Get<bool>(
            RankingServerUrl,
            new Dictionary<string, string>()
            {
                { "score", record.Score.ToString() }
            }
        );
    }
}
