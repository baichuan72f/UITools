using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json.Linq;
using QF.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class TextGroupComponent : MonoBehaviour
{
    public Text[] Texts;
    
    Dictionary<string, Text> template;

    private Dictionary<string, string> dataWay;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void setData(JObject data)
    {
        if (template == null)
        {
            template = new Dictionary<string, Text>();
            for (int i = 0; i < Texts.Length; i++)
            {
                if (Texts[i] == null) continue;
                if (string.IsNullOrEmpty(Texts[i].name)) continue;
                if (template.ContainsKey(Texts[i].name)) continue;
                template.Add(Texts[i].name, Texts[i]);
            }
        }
        foreach (var item in template)
        {
            if (item.Value != null)
            {
                if (!string.IsNullOrEmpty(item.Key) && dataWay.ContainsKey(item.Key))
                {
                    var result = dataWay[item.Key];
                    if (!string.IsNullOrEmpty(result) && data.JsonDataContainsKey(result))
                    {
                        item.Value.text = data[result].ToString();
                    }
                }
            }
        }
    }
}