using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using QFramework;
using UniRx;
using UnityEngine;

namespace UniRxIoC.Base
{
    public class NetWorkService : INetWorkService
    {
        public void RequestSomeData(string url, Action<string> onResponse)
        {
            url.LogInfo();
            // 延时一秒，用来请求网络数据
            Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
            {
                var result = "数据请求成功";
                var data = new JObject();
                data.Add("data", result);
                onResponse(data.ToString());
            });
        }
    }

    public interface INetWorkService
    {
        void RequestSomeData(string url, Action<string> onResponse);
    }
}