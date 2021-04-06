using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public static class ProxyInterface
{
    public const string http = "http://";
    public const string https = "httsp://";
    public const bool isHttp = true;
    public const string ip = "60.2.7.228";

    public const string port = "6009";

    public const string domain = "";

    public const string login = "/login";

    public const string addPlan = "/doublec/plan/addPlan";
    public const string listNo = "/doublec/hiddendanger/listNo";

    public static string ConfigFullUrl(this string realtiveUrl)
    {
        return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", ip, port, domain, realtiveUrl);
    }


    public static IObservable<string> GetResult(this HttpClient client, HttpRequestMessage message)
    {
        return Observable.Start<string>(() => GetHttpResult(client, message).Result);
    }

    static async Task<string> GetHttpResult(this HttpClient client, HttpRequestMessage message)
    {
        var response = await client.SendAsync(message);
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
}