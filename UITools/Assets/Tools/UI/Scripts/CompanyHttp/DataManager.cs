using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace CompanyHttp
{
    public class DataManager
    {
        public static Dictionary<string, RequestItem> RequestItems=new Dictionary<string, RequestItem>();
    }

    public class RequestItem
    {
        public DateTime Time;
        public TimeSpan TimeSpan;
        public string Url;
        public JsonData Data;
        public List<IRequestViewer> Viewers;
    }
}