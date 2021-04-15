using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
/// <summary>
/// 禁止在此类中填入数据
/// </summary>
public static class HomePanelView 
{
    //查询主页数据
    public static Dictionary<string,string> ShowHomePage()
    {
        var dic = new Dictionary<string, string>();
        return dic;
    }
    /// <summary>
    /// 打开历史记录
    /// </summary>
    /// <param name="streamName">记录名</param>
    /// <param name="state">状态</param>
    /// <param name="timeSpan">时长</param>
    public static JObject OpenHistory(string streamName,string state,TimeSpan timeSpan)
    {
			Debug.Log(streamName+state+TimeSpan.TicksPerDay);
            return null;
    }
}
