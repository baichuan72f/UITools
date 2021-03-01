using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sender : MonoBehaviour
{
    public string url;

    public int time;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (time == 2)
        {
            SendRequest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendRequest(Action<string> callback = null)
    {
        if (!Controller.Instance.dataDic.ContainsKey(url))
        {
            Controller.Instance.dataDic.Add(url, "");
        }

        Controller.Instance.dataDic[url] = "Result:" + Random.value;
        if (callback != null)
        {
            callback("Result:" + Random.value);
        }
    }
}