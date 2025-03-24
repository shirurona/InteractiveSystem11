using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpService : MonoBehaviour
{
    /// <summary>
    /// サーバへGETリクエストを送信
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="requestParams"></param>
    /// <returns></returns>
    public static async UniTask<T> Get<T>(string url, IDictionary<string, string> requestParams = null)
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
            requestUrl = requestUrl.Remove(0, requestUrl.Length - 1);
        }
 
        using var request = UnityWebRequest.Get(requestUrl.ToString());
 
        // リクエスト送信
        await request.SendWebRequest().ToUniTask();
 
        // エラー判定
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError) {
            throw new Exception($"通信に失敗しました。({request.error})");
        }
        
        return JsonUtility.FromJson<T>(request.downloadHandler.text);
    }
 
    /// <summary>
    /// サーバへPOSTリクエストを送信
    /// </summary>
    /// <param name="url"></param>
    /// <param name="requestParams"></param>
    /// <returns></returns>
    public static async UniTask Post(string url, IDictionary<string, string> requestParams)
    {
        using var request = UnityWebRequest.Post(url, (Dictionary<string, string>)requestParams);
 
        // リクエスト送信
        await request.SendWebRequest().ToUniTask();
 
        // エラー判定
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError) {
            throw new Exception($"通信に失敗しました。({request.error})");
        }
    }
}
