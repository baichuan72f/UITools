using System;
using System.Collections.Generic;
using System.Net.Http;
using Common.Interface;
using NetWork;
using Newtonsoft.Json.Linq;
using RestSharp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Tester_Proxy : MonoBehaviour
{
    private RestSharpProxy m_Proxy;
    public string userName = "16833336666";
    public string passWord = "111111";

    public static Dictionary<string, BindItem> bindDic = new Dictionary<string, BindItem>();

    //请求刷新时间
    public float updateSpan = 3f;
    private float _nextTime;

    //实例化
    public GameObject item;

    public int count = 200;

    // Start is called before the first frame update
    void Start()
    {
        RestSharpProxy.Instance.init(userName, passWord);
        m_Proxy = RestSharpProxy.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < count; i++)
            {
                var url = string.Format(INetWork.EventReportById, i + 50).ConfigFullUrl();
                Instantiate(item, item.transform.parent).AddComponent<ViewItem>().path = url;
            }

            Debug.Log(item.transform.parent.childCount);
        }

        //节流刷新数据
        if (CanUpdate())
        {
            foreach (var bindItem in bindDic)
            {
                //get
                if (bindItem.Value.method == Method.GET.ToString())
                {
                    //异步发送无参请求
                    RestSharpProxy.Instance.AsynGet(bindItem.Value.url);
                }

                //post
                if (bindItem.Value.method == Method.POST.ToString())
                {
                    //异步发送无参请求
                    RestSharpProxy.Instance.AsynPost(bindItem.Value.url);
                }
            }
        }
    }

    public static int Bind(string targetIndex, BindItem value)
    {
        if (string.IsNullOrEmpty(targetIndex))
        {
            return -1;
        }

        if (!bindDic.ContainsKey(targetIndex))
        {
            bindDic.Add(targetIndex, value);
            return 0;
        }
        else
        {
            bindDic[targetIndex] = value;
            return 1;
        }

        //return -1;
    }

    public static int UnBind(string targetIndex)
    {
        if (!string.IsNullOrEmpty(targetIndex) && bindDic.ContainsKey(targetIndex))
        {
            bindDic.Remove(targetIndex);
            return 0;
        }

        return -1;
    }

    //节流阀
    public bool CanUpdate()
    {
        if (Time.time < _nextTime) return false;
        _nextTime = Time.time + updateSpan;
        return true;
    }
}

public struct BindItem
{
    public string url;
    public string method;
    public string[] keys;
    public GameObject target;
    public Action<GameObject, string, string[]> callBack;

    public BindItem(string url, string[] keys, GameObject target, Action<GameObject, string, string[]> callBack = null,
        string method = "GET")
    {
        this.url = url;
        this.method = method;
        this.keys = keys;
        this.target = target;
        this.callBack = callBack;
    }
}