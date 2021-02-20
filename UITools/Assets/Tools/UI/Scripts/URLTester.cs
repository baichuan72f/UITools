using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLTester : MonoBehaviour
{
    public string[] Url;
    public string[] UserInfo;
    public string[] Address;
    public string[] DeviceInfo;
    public bool UseEncode;
    public Dictionary<string, string[]> ParamDic;

    public int Count;
    // Start is called before the first frame update
    void Start()
    {
        ParamDic.Add("Url",Url);
        ParamDic.Add("UserInfo",UserInfo);
        ParamDic.Add("Address",Address);
        ParamDic.Add("DeviceInfo",DeviceInfo);
        Count = Url.Length * UserInfo.Length * Address.Length * DeviceInfo.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
