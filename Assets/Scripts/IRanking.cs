using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IRanking
{ 
    public UniTask SendScoreAsync(Record record);
    public UniTask<List<Record>> GetRankingAsync();
    public UniTask<bool> IsRankedInAsync(Record record);
}