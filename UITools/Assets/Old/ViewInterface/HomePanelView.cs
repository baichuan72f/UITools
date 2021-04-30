using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Interface;
using NetWork;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// 禁止在此类中填入数据
/// </summary>
public static class HomePanelView
{
    //查询主页数据
    public static JObject ShowHomePage()
    {
        string url = INetWork.IndexData.ConfigFullUrl();
        //请求数据 
        RestSharpProxy.Instance.Get(url);
        var result = DataCache.instance.GetValue(url);
        Debug.Log(result);
        if (string.IsNullOrEmpty(result))
        {
            return null;
        }

        return JObject.Parse(result);
    }

    /// <summary>
    /// 打开历史记录
    /// </summary>
    /// <param name="streamName">记录名</param>
    /// <param name="state">状态</param>
    /// <param name="timeSpan">时长</param>
    public static string GetStreamInfoList(string streamName, int state, int daySpan)
    {
        //配置Url
        string url = (state == -2 && daySpan == 30)
            ? GetListBystatusUrl(streamName, state, daySpan)
            : GetListUrl(streamName, state, daySpan);
        //请求数据 
        RestSharpProxy.Instance.Get(url);
        var result = DataCache.instance.GetValue(url);
        Debug.Log(result);
        if (string.IsNullOrEmpty(result))
        {
            return null;
        }

        return result;
    }

    /// <summary>
    /// 获取4列表请求Url
    /// </summary>
    /// <param name="streamName"></param>
    /// <param name="state"></param>
    /// <param name="daySpan">查询多少天之前的数据</param>
    /// <returns></returns>
    public static string GetListUrl(string streamName, int state, int daySpan)
    {
        //计算基础地址
        string url = INetWork.EventReportList;
        switch (streamName)
        {
            case "EventReport":
                url = INetWork.EventReportList;
                break;
            case "ItemAlarm":
                url = INetWork.ItemAlarmList;
                break;
            case "InspectionReport":
                url = INetWork.InspectionReportList;
                break;
            case "ModuleHiddenDanger":
                url = INetWork.ModuleHiddenDangerList;
                break;
        }

        //添加时间状态参数
        var param = new JObject();
        param = GetTimeParam(param, streamName, state, daySpan);
        param.Add("status", state);
        //配置完整地址
        url = url.ConfigGetUrlWithParam(param);
        Debug.Log(url);
        return url;
    }

    private static JObject GetTimeParam(JObject param, string streamName, int state, int daySpan)
    {
        var startTime = DateTime.Now.AddDays(-1) + TimeSpan.FromDays(-daySpan);
        var endTime = DateTime.Now.AddDays(1);

        var keyStr = streamName == "ItemAlarm" ? "alarmTime" : "discoveryTime";
        var timeParam = new JObject();
        timeParam.Add("startTime", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
        timeParam.Add("endTimes", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add(keyStr, timeParam);
        return param;
    }

    /// <summary>
    /// 获取4列表请求Url
    /// </summary>
    /// <param name="streamName"></param>
    /// <param name="state"></param>
    /// <param name="daySpan">查询多少天之前的数据</param>
    /// <returns></returns>
    public static string GetListBystatusUrl(string streamName, int state, int daySpan)
    {
        //计算基础地址
        string url = INetWork.EventReportListByStatus;
        switch (streamName)
        {
            case "EventReport":
                // 事件：状态0待受理，1已受理/待指派，2已指派/待接收，3已接收/处理中，4已处理/待复查,5已复查合格，6复查不合格
                url = INetWork.EventReportListByStatus;
                break;
            case "ItemAlarm":
                // 设备报警：状态0待受理，1已受理/待指派，2已指派/待接收，3已接收/处理中，4已处理
                url = INetWork.ItemAlarmListByStatus;
                break;
            case "InspectionReport":
                // 巡查：0待受理，1已受理/待指派，2已指派/待接收，3已接收/处理中，4已处理
                url = INetWork.InspectionReportListByStatus;
                break;
            case "ModuleHiddenDanger":
                // 隐患：状态0待受理，1已受理/待指派，2已指派/待接收，3已接收/处理中，4已处理/待复查,5已复查（复查合格），6复查不合格
                url = INetWork.ModuleHiddenDangerListByStatus;
                break;
        }

        //添加时间状态参数
        var param = new JObject();
        var strList = new List<string>();
        var statusArrayIndexs = 4;
        if (state == -2)
        {
            for (int i = 0; i < statusArrayIndexs; i++)
            {
                strList.Add("statusArray=" + i);
            }
        }
        else
        {
            strList.Add("statusArray=" + -1);
        }

        param = GetTimeParam(param, streamName, state, daySpan);
        //配置完整地址
        url = url.ConfigGetUrlWithParam(param) + "&" + string.Join("&", strList);
        Debug.Log(url);
        return url;
    }

    /// <summary>
    /// 获取4列表单项的详情
    /// </summary>
    /// <param name="streamName"></param>
    /// <param name="state"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static JObject GetStreamInfo(string streamName, int id)
    {
        string url = GetInfoUrl(streamName, id);

        //请求数据 
        RestSharpProxy.Instance.Get(url);
        var result = DataCache.instance.GetValue(url);
        Debug.Log(result);
        if (string.IsNullOrEmpty(result))
        {
            return null;
        }

        return JObject.Parse(result);
    }

    /// <summary>
    /// 获取4列表单项的详情Url
    /// </summary>
    /// <param name="streamName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static string GetInfoUrl(string streamName, int id)
    {
        string url = INetWork.EventReportById;
        switch (streamName)
        {
            case "EventReport":
                url = INetWork.EventReportById;
                break;
            case "ItemAlarm":
                url = INetWork.ItemAlarmById;
                break;
            case "InspectionReport":
                url = INetWork.InspectionReportById;
                break;
            case "ModuleHiddenDanger":
                url = INetWork.ModuleHiddenDangerById;
                break;
        }

        url = string.Format(url, id).ConfigFullUrl();
        Debug.Log(url);
        return url;
    }
}