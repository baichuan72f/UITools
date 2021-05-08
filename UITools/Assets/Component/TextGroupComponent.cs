using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json.Linq;
using QF.Extensions;
using QFramework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextGroupComponent : MonoBehaviour
{
    public Text[] texts;
    [HideInInspector] public string template;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadTemplate().LogInfo();
        }
    }

    public JObject CreatTemplate2(GameObject target)
    {
        var tmp = new JObject();
        var elements = GetComponentsInChildren<UIElement>(true);
        Dictionary<UIElement, JObject> elementDic = new Dictionary<UIElement, JObject>();
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].GetComponentsInChildren<Bind>() != null)
            {
                elementDic.Add(elements[i], new JObject());
            }
        }

        var binds = GetComponentsInChildren<Bind>(true);
        for (int i = 0; i < binds.Length; i++)
        {
            var bind = binds[i];
            if (bind.GetComponent<Text>() == null)
            {
                continue;
            }

            var parent = bind.GetComponentInParent<UIElement>();
            if (parent != null && elementDic.ContainsKey(parent))
            {
                if (!elementDic[parent].ContainsKey(bind.name))
                {
                    elementDic[parent].Add(bind.name, bind.GetComponent<Text>().text);
                }
            }
            else
            {
                if (!tmp.ContainsKey(bind.name))
                {
                    tmp.Add(bind.name, bind.GetComponent<Text>().text);
                }
            }
        }

        for (int i = 0; i < elements.Length; i++)
        {
            var element = elements[elements.Length - i - 1];
            var parent = element.transform.parent;
            var parentElement = element.GetComponentInParent<UIElement>();
            if (parent != null && parentElement != null && elementDic.ContainsKey(parentElement))
            {
                if (!elementDic[parentElement].ContainsKey(element.name))
                {
                    elementDic[parentElement].Add(element.name, elementDic[element]);
                }
            }
            else
            {
                if (!tmp.ContainsKey(element.name))
                {
                    tmp.Add(element.name, elementDic[element]);
                }
            }
        }

        return tmp;
    }

    [ContextMenu("LoadTemplate")]
    public void ShowTemplateStr()
    {
        template = LoadTemplate().ToString();
    }

    public JObject LoadTemplate()
    {
        if (texts == null)
        {
            return null;
        }

        var tmp = new JObject();
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] == null) continue;
            if (string.IsNullOrEmpty(texts[i].name)) continue;
            if (tmp.ContainsKey(texts[i].name)) continue;
            tmp.Add(texts[i].name, texts[i].text);
        }

        return tmp;
    }
}