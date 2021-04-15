using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Advanced.Algorithms.DataStructures;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class JsonView : MonoBehaviour
{
    public JObject data;

    // Start is called before the first frame update
    void Start()
    {
        data = new JObject();
        // 展示数据
        ShowData(data);
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 展示数据的方法
    /// </summary>
    /// <param name="data"></param>
    public void ShowData(JObject data)
    {
        for (int i = 0; i < data.Values().Count(); i++)
        {
            var token = data.GetValue(data.PropertyValues()[i].ToString());
            switch (token.Type)
            {
                case JTokenType.None:
                    break;
                case JTokenType.Object:
                    break;
                case JTokenType.Array:
                    break;
                case JTokenType.Constructor:
                    break;
                default:
                    
                    break;
            }
        }
    }
}