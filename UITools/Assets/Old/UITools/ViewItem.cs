using System.Collections;
using System.Collections.Generic;
using Common;
using NetWork;
using Newtonsoft.Json.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ViewItem : MonoBehaviour
{
    public string path;

    public string[] keyStrings;

    private Text _text;

    public float updateSpan = 0.35f;

    private float _nextTime = 0;

    public string method = "GET";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CanUpdate())
        {
            //绑定更新（现在实现为强制刷新绑定）
            Bind(_text.gameObject, path, keyStrings);
            //刷新视图
            updateData(_text.gameObject, path, keyStrings);
        }
    }

    public bool CanUpdate()
    {
        //节流阀
        if (Time.time < _nextTime) return false;
        _nextTime = Time.time + updateSpan;

        return true;
    }

    public void Bind(GameObject target, string url, string[] keys)
    {
        //绑定数据
        var bind = new BindItem(url, keys, target, updateData, method);
        Tester_Proxy.Bind(url, bind);
    }

    public void updateData(GameObject target, string url, string[] keys)
    {
        //更新数据
        var result = DataCache.instance.GetValue(url);
        //合法性校验
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogWarning("请求结果为空");
            return;
        }

        var data = JObject.Parse(result);
        if (keys == null)
        {
            Debug.LogWarning("keys为空，组件未更新");
            return;
        }

        //按路径查找数据
        var str = data.Root;
        int i;
        for (i = 0; i < keys.Length; i++)
        {
            if (string.IsNullOrEmpty(str.ToString()))
            {
                Debug.LogWarning("keys路径未找到，组件未更新");
                break;
            }

            str = str[keys[i]];
        }

        //获取组件
        _text = target.GetComponent<Text>();
        if (!_text) return;
        //如果查找到数据则更新组件值
        if (i == keys.Length)
        {
            _text.text = str.ToString();
        }
    }
}