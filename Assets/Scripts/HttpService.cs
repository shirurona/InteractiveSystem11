using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpService
{
    /// <summary>
    /// サーバへGETリクエストを送信
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="requestParams"></param>
    /// <returns></returns>
    public static async UniTask<T> GetAsync<T>(string url, IDictionary<string, string> requestParams = null)
    {
        StringBuilder requestUrl = new StringBuilder(url);
 
        // リクエストパラメータがある場合はURLに結合
        if (requestParams != null) {
            requestUrl.Append("?");
 
            // パラメータ文keyとvalueを結合
            foreach (var requestParam in requestParams) {
                requestUrl.Append($"{requestParam.Key}={requestParam.Value}&");
            }
 
            // 後ろの&を削除
            requestUrl = requestUrl.Remove(requestUrl.Length - 1, 1);
        }
        
        Debug.Log(requestUrl.ToString());
        using var request = UnityWebRequest.Get(requestUrl.ToString());
        // リクエスト送信
        await request.SendWebRequest().ToUniTask();
        // エラー判定
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError) {
            throw new Exception($"通信に失敗しました。({request.error})");
        }
        Debug.Log(request.downloadHandler.text);
        
        // JsonUtilityではプロパティのデシリアライズに失敗するのでSystem.Text.Jsonを利用
        return JsonSerializer.Deserialize<T>(request.downloadHandler.text);
    }
 
    /// <summary>
    /// サーバへPOSTリクエストを送信
    /// </summary>
    /// <param name="url"></param>
    /// <param name="requestParams"></param>
    /// <returns></returns>
    public static async UniTask PostAsync(string url, Dictionary<string, string> requestParams)
    {
        using var request = UnityWebRequest.Post(url, requestParams);
        // リクエスト送信
        await request.SendWebRequest().ToUniTask();
 
        // エラー判定
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError) {
            throw new Exception($"通信に失敗しました。({request.error})");
        }
    }
    
    /// <summary>
    /// サーバへ接続できるか確認する
    /// </summary>
    /// <param name="url"></param>
    /// <param name="timeOutSeconds"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async UniTask<bool> CheckConnectionAsync(string url, int timeOutSeconds, CancellationToken ct)
    {
        using var request = new UnityWebRequest(url) { timeout = timeOutSeconds };
        var operation = request.SendWebRequest();
        await UniTask.WaitUntil(() => operation.isDone, cancellationToken: ct);
        // 404NotFoundだとProtocolErrorが出るので許す
        return request.result == UnityWebRequest.Result.Success || request.result == UnityWebRequest.Result.ProtocolError;
    }
}
