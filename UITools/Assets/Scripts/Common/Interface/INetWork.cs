using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

namespace Common.Interface
{
    public static class INetWork
    {
        public const string http = "http://";
        public const string https = "httsp://";
        public const string ip = "60.2.7.228";
        public const string local = "127.0.0.1";

        public const string port = "6009";
        public const string jsonServerPort = "3000";

        public const string domain = "";

        public const string login = "/login";

        public const string addPlan = "/doublec/plan/addPlan";
        public const string listNo = "/doublec/hiddendanger/listNo";
        public const string taskList = "/doublec/task/list";

        //配置无参数网络地址（将相对路径转为绝对路径）
        public static string ConfigFullUrl(this string realtiveUrl, bool isHttp = true)
        {
            //return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", local, jsonServerPort, domain, realtiveUrl);
            return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", ip, port, domain, realtiveUrl);
        }

        //配置网络地址（将相对路径转为绝对路径）
        public static string ConfigGetUrlWithParam(this string realtiveUrl, JObject param, bool isHttp = true)
        {
            StringBuilder builder = new StringBuilder(realtiveUrl);
            if (param != null && param.Count > 0)
            {
                builder.Append("?");
                foreach (var item in param)
                {
                    builder.Append(item.Key + "=" + item.Value);
                }
            }

            return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", ip, port, domain, builder.ToString());
        }

        //延迟1.5秒获取数据
        public static string GetResult(this HttpClient client, HttpRequestMessage message, int milliseconds = 1500)
        {
            return GetResultAsyn(client, message).Wait(TimeSpan.FromMilliseconds(milliseconds));
        }

        //异步获取数据
        public static IObservable<string> GetResultAsyn(this HttpClient client, HttpRequestMessage message)
        {
            return Observable.Start<string>(() => GetHttpResult(client, message).Result);
        }

        //发送网络请求
        public static async Task<string> GetHttpResult(this HttpClient client, HttpRequestMessage message)
        {
            HttpResponseMessage response;
            if (message.Method == HttpMethod.Get)
            {
                response = await client.GetAsync(message.RequestUri);
            }
            else
            {
                response = await client.SendAsync(message);
            }

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}