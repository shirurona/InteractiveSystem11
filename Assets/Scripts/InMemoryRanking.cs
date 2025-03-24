using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using SQLite;
using UnityEngine;

public class InMemoryRanking : IRanking
{
    SQLiteAsyncConnection db;
    string nameColumn = "Name";
    string scoreColumn = "Score";
    string tableName = "Record";
    int limit = 5;

    public InMemoryRanking(string dbPath)
    {
        db = new SQLiteAsyncConnection(dbPath);
    }
    
    public async UniTask SendScoreAsync(Record record)
    {
        Debug.Log("send");
        await db.CreateTableAsync<Record>();
        await db.InsertAsync(record);
    }

    public async UniTask<List<Record>> GetRankingAsync()
    {
        await db.CreateTableAsync<Record>();
        return await db.QueryAsync<Record>($@"
            SELECT {nameColumn}, MAX({scoreColumn}) AS {scoreColumn}
            FROM {tableName}
            WHERE {nameColumn} IS NOT NULL
            GROUP BY {nameColumn}
            ORDER BY {scoreColumn} DESC
            LIMIT {limit}
            ");
    }

    public async UniTask<bool> IsRankedInAsync(Record record)
    {
        await db.CreateTableAsync<Record>();
        // 5位のスコアを取得
        Record minRankedScore = await db.FindWithQueryAsync<Record>($@"
            SELECT MIN(score) AS score FROM (
            SELECT {scoreColumn} FROM {tableName} 
            WHERE {nameColumn} IS NOT NULL 
            GROUP BY {nameColumn} 
            ORDER BY {scoreColumn} DESC 
            LIMIT {limit})
            ");
        return record.Score >= minRankedScore.Score;
    }
}
