using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Common;
using Common.Interface;
using LitJson;
using Newtonsoft.Json.Linq;
using RestSharp;
using UnityEngine;

namespace NetWork
{
    public class RestSharpProxy
    {
        private static RestSharpProxy instance;

        public static RestSharpProxy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RestSharpProxy();
                }

                return instance;
            }
            set { instance = value; }
        }

        //储存的token
        public string loginToken = string.Empty;


        public void init(string userName, string passWord)
        {
            Login(INetWork.login.ConfigFullUrl(), userName, passWord);
            Instance = this;
        }

        public void Login(string url, string username, string password)
        {
            // 登录请求
            var jobect = new JObject();
            string md5Pwd = Md5Sum(password);
            jobect.Add("password", md5Pwd);
            jobect.Add("phonenumber", username);
            var response = Post(url, jobect.ToString());

            // 获取返回信息并检查
            // 反序列话
            JsonData data = JsonMapper.ToObject(response);
            // 检查返回的代码
            string code = "404";
            if (data["code"] != null)
            {
                // 检查返回的代码
                code = data["code"].ToString();
            }


            if (code.Equals("200"))
            {
                loginToken = data["token"].ToString();
                Debug.Log("成功登录Token: " + loginToken);
            }
            else
            {
                Debug.Log("登录失败");
                loginToken = string.Empty;
                Logout(INetWork.loginOut.ConfigFullUrl());
            }
        }

        public static string Md5Sum(string s)
        {
            var h = new MD5CryptoServiceProvider();
            h.Initialize();
            byte[] data = h.ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public void Logout(string url)
        {
            // 检查登录的Token
            if (string.IsNullOrEmpty(loginToken))
            {
                Debug.LogWarning("登出请求没有有效的登录Token。");
            }

            // 登出请求
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", loginToken);
            IRestResponse response = client.Execute(request);

            // 获取返回信息并检查
            // 反序列话
            JsonData data = JsonMapper.ToObject(response.Content);

            // 检查返回的代码
            string code = data["code"].ToString();

            if (code.Equals("200"))
            {
                loginToken = string.Empty;
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public string Get(string url, Dictionary<string, object> parameter = null,
            int timeout = 2500, string contentType = "application/x-www-form-urlencoded")
        {
            //1.检查地址
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning("url empty");
                return String.Empty;
            }

            //2.配置请求为Get请求
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.Timeout = timeout;

            if (!string.IsNullOrEmpty(loginToken))
                request.AddHeader("token", loginToken);
            //3.添加参数
            // 添加请求参数
            if (parameter != null && parameter.Count > 0)
            {
                request.AddHeader("Content-Type", contentType);
                foreach (string key in parameter.Keys)
                    request.AddParameter(key, parameter[key].ToString());
            }

            //4.执行请求
            var r = client.Execute(request);
            if (!string.IsNullOrEmpty(r.Content))
            {
                DataCache.instance.SetValue(client.BaseUrl, r.Content);
                return r.Content;
            }

            return String.Empty;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string Post(string url, string param = null, int timeout = 2500)
        {
            //1.检查地址
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning("url empty");
                return String.Empty;
            }

            //2.配置请求为Post
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.Timeout = timeout;

            if (!string.IsNullOrEmpty(loginToken))
                request.AddHeader("token", loginToken);
            //3.添加参数
            if (!string.IsNullOrEmpty(param))
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", param, ParameterType.RequestBody);
            }

            //4.执行请求
            var r = client.Execute(request);
            if (!string.IsNullOrEmpty(r.Content))
            {
                DataCache.instance.SetValue(client.BaseUrl, r.Content);
                return r.Content;
            }

            return String.Empty;
        }

        /// <summary>
        /// 异步Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        public void AsynGet(string url, Dictionary<string, object> parameter = null, Action<string> callback = null,
            int timeout = 2500, string contentType = "application/x-www-form-urlencoded")
        {
            //1.检查地址
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning("url empty");
                return;
            }

            //2.配置请求为Get请求
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.Timeout = timeout;

            if (!string.IsNullOrEmpty(loginToken))
                request.AddHeader("token", loginToken);
            //3.添加参数
            // 添加请求参数
            if (parameter != null && parameter.Count > 0)
            {
                request.AddHeader("Content-Type", contentType);
                foreach (string key in parameter.Keys)
                    request.AddParameter(key, parameter[key].ToString());
            }

            //4.执行请求
            client.ExecuteAsync(request, (r, h) =>
            {
                Debug.Log(r.Content);
                if (!string.IsNullOrEmpty(r.Content))
                {
                    DataCache.instance.SetValue(client.BaseUrl, r.Content);
                    callback?.Invoke(r.Content);
                }
            });
        }

        /// <summary>
        /// 异步Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public void AsynPost(string url, string param = null, Action<string> callback = null, int timeout = 2500)
        {
            //1.检查地址
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning("url empty");
                return;
            }

            //2.配置请求为Post
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.Timeout = timeout;

            if (!string.IsNullOrEmpty(loginToken))
                request.AddHeader("token", loginToken);
            //3.添加参数
            if (!string.IsNullOrEmpty(param))
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", param, ParameterType.RequestBody);
            }

            //4.执行请求
            client.ExecuteAsync(request, (r, h) =>
            {
                if (h.WebRequest.HaveResponse)
                {
                    if (!string.IsNullOrEmpty(r.Content))
                    {
                        DataCache.instance.SetValue(client.BaseUrl, r.Content);
                        callback?.Invoke(r.Content);
                    }
                }
            });
        }
    }
}