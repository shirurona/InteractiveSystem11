using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "RankingFactory", menuName = "Scriptable Objects/RankingFactory")]
public class RankingFactory : ScriptableObject
{
    [SerializeField] private string databaseName = "local_ranking.db";
    [SerializeField] private string checkUrl = "http://192.168.210.105/";
    [SerializeField] private int timeOutSeconds = 2;

    public async UniTask<IRanking> CreateRanking(CancellationToken ct)
    {
        bool canConnect = await HttpService.CheckConnectionAsync(checkUrl, timeOutSeconds, ct);
        if (canConnect)
        {
            return new ServerRanking();
        }
        else
        {
            return new InMemoryRanking(System.IO.Path.Combine(Application.persistentDataPath, databaseName));
        }
    }
}
