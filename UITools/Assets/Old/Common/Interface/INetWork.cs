using System;
using System.Collections.Generic;
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
        //public const string ip = "192.168.30.56";
        //public const string local = "192.168.30.56";

        public const string port = "6009";
        public const string jsonServerPort = "3000";

        public const string domain = "";

        public const string login = "/login";
        public const string loginOut = "/loginOut";

        public const string addPlan = "/doublec/plan/addPlan";
        public const string listNo = "/doublec/hiddendanger/listNo";
        public const string taskList = "/doublec/task/list";

        #region MyRegion

        //首页接口
        public const string IndexData = "/system/home/indexData";

        #endregion

        #region 4列表

        //事件上报记录列表
        public const string EventReportList = "/doublec/event_r/list";

        //隐患排查记录列表
        public const string ModuleHiddenDangerList = "/doublec/hiddendanger/list";

        //设备报警记录列表
        public const string ItemAlarmList = "/monitor/alarm/list";

        //日常巡检记录列表
        public const string InspectionReportList = "/daily/iesDailyInspectionReport/list";

        //事件上报记录列表(带参)
        public const string EventReportListByStatus = "/doublec/event_r/listBystatus?{0}";

        //隐患排查记录列表(带参)
        public const string ModuleHiddenDangerListByStatus = "/doublec/hiddendanger/listBystatus";

        //设备报警记录列表(带参)
        public const string ItemAlarmListByStatus = "/monitor/alarm/listByAlarmTime";

        //日常巡检记录列表(带参)
        public const string InspectionReportListByStatus = "/daily/iesDailyInspectionReport/listBystatus";

        #endregion

        #region 4详情

        //事件上报记录列表
        public const string EventReportById = "/doublec/event_r/{0}";

        //隐患排查记录列表
        public const string ModuleHiddenDangerById = "/doublec/hiddendanger/{0}";

        //设备报警记录列表
        public const string ItemAlarmById = "monitor/alarm/selectById/{0}";

        //日常巡检记录列表
        public const string InspectionReportById = "/daily/iesDailyInspectionReport/{0}";

        #endregion

        //配置无参数网络地址（将相对路径转为绝对路径）
        public static string ConfigFullUrl(this string realtiveUrl, bool isHttp = true)
        {
            //return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", local, jsonServerPort, domain, realtiveUrl);
            return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", ip, port, domain, realtiveUrl);
        }

        //配置网络地址（将相对路径转为绝对路径）
        public static string ConfigGetUrlWithParam(this string realtiveUrl, JObject param, bool isHttp = true)
        {
            var builder = new List<string>();
            var result = string.Empty;
            if (param != null && param.Count > 0)
            {
                result = "?";
                foreach (var item in param)
                {
                    builder.Add(item.Key + "=" + item.Value);
                }
            }

            result += string.Join("&", builder);
            return string.Format((isHttp ? http : https) + "{0}:{1}{2}{3}", ip, port, domain, realtiveUrl + result);
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