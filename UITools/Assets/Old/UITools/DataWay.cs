using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DataWay
{
    public DataWayType WayType;
    public float timeSpan;
    public Func<JObject> GeyWay;
    public Action<JObject> SetWay;
    public Func<JObject, JObject> DataConvert;
    private JObject m_Cache;

    public void Update(TimeSpan timeSpan)
    {
        if (WayType == DataWayType.None)
        {
            return;
        }

        if (WayType == DataWayType.TimeSpan)
        {
            if (GeyWay != null) m_Cache = GeyWay();
            return;
        }

        if (WayType == DataWayType.Call)
        {
            m_Cache = GeyWay?.Invoke();
            var result = DataConvert?.Invoke(m_Cache);
            if (result != null) SetWay?.Invoke(result);
            return;
        }
    }
}

public enum DataWayType
{
    None,
    TimeSpan,
    Call
}