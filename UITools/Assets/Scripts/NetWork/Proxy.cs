using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Common;
using Common.Interface;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NetWork
{
    public class Proxy
    {
        private bool isLogin;
        private string token;
        private ServiceProvider serviceProvider;
        private IHttpClientFactory httpClientFactory;
        private HttpClient client;

        public Proxy(string username, string password)
        {
            serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            client = httpClientFactory.CreateClient();
            if (!isLogin)
            {
                token = Login(username, password).ToString();
                isLogin = !string.IsNullOrEmpty(this.token);
                Debug.Log("token : " + token);
            }
        }

        public JToken Login(string username, string password)
        {
            var jObject = new JObject();
            var md5 = new MD5CryptoServiceProvider();
            md5.Initialize();
            var _byteRst = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder SBuilder = new StringBuilder();
            for (int i = 0; i < _byteRst.Length; i++)
            {
                SBuilder.Append(_byteRst[i].ToString("x2"));
            }

            jObject.Add("password", SBuilder.ToString());
            jObject.Add("phonenumber", username);
            var message = new HttpRequestMessage();
            message.RequestUri = new Uri(INetWork.login.ConfigFullUrl());
            message.Headers.Add("Accept", "application/json");
            message.Content = new StringContent(jObject.ToString(), System.Text.Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            var r = client.GetResult(message);
            var result = JsonConvert.DeserializeObject<JObject>(r);
            var t = result["token"];
            return t;
        }

        public JObject SendJsonRequest(string url, HttpMethod method, JObject param = null,
            string accept = "application/json")
        {
            var message = new HttpRequestMessage();
            message.RequestUri = new Uri(url);
            message.Headers.Add("Accept", accept);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            message.Headers.Add("Cookie", "Admin-Token=" + token);
            // eyJhbGciOiJIUzUxMiJ9.eyJsb2dpbl91c2VyX2tleSI6IjhiMTVmMDE0LTU4ZGUtNDQ0Ny05NTc1LTBiZTYwZWQzNDQ1MSJ9.nGlJHBZ5TZ8_JnFmVZ76MxW4C6I5Txc0Xsc4kMRnxXDY4z-dD_dpwqL0KwhnYavLvRIdSE487uxkQoyRjoXYOQ
            if (param != null)
            {
                message.Content = new StringContent(param.ToString(), System.Text.Encoding.UTF8, accept);
            }

            message.Method = method;

            var r = client.GetResult(message);
            var result = JsonConvert.DeserializeObject<JObject>(r);
            DataCache.instance.SetValue(message.RequestUri.AbsolutePath, result);
            Debug.Log("Set: " + message.RequestUri.AbsolutePath);
            Debug.Log(result.ToString());
            return result;
        }
    }
}