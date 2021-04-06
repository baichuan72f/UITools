using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class DataProxy
{
    private bool isLogin;
    private string token;
    private ServiceProvider serviceProvider;
    private IHttpClientFactory httpClientFactory;
    private HttpClient client;

    public DataProxy(string username, string password)
    {
        serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
        httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        client = httpClientFactory.CreateClient();
        if (!isLogin)
        {
            token = Login(username, password).Result;
            isLogin = !string.IsNullOrEmpty(this.token);
            Debug.Log("token : " + token);
        }
    }

    public async Task<string> Login(string username, string password)
    {
        var jObject = new JObject();
        jObject.Add("agent", null);
        jObject.Add("code", null);
        jObject.Add("password", password);
        jObject.Add("phonenumber", username);
        jObject.Add("username", null);
        jObject.Add("uuid", null);
        var message = new HttpRequestMessage();
        message.RequestUri = new Uri(ProxyInterface.login.ConfigFullUrl());
        message.Headers.Add("Accept", "application/json");
        message.Content = new StringContent(jObject.ToString(), System.Text.Encoding.UTF8, "application/json");
        message.Method = HttpMethod.Post;
        var r = client.GetResult(message).Wait();
        var result = JsonConvert.DeserializeObject<JObject>(r);
        var t = result["token"].ToString();
        return t;
    }

    public async Task<string> listNo(JObject param)
    {
        var message = new HttpRequestMessage();
        message.RequestUri = new Uri(ProxyInterface.listNo.ConfigFullUrl());
        message.Headers.Add("Accept", "application/json");
        if (param != null)
        {
            message.Content = new StringContent(param.ToString(), System.Text.Encoding.UTF8, "application/json");
        }
        message.Method = HttpMethod.Post;
        var r = client.GetResult(message).Wait();
        var result = JsonConvert.DeserializeObject<JObject>(r);
        var t = result["data"].ToString();
        return t;
    }
}