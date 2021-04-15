using System.Net.Http;
using Common.Interface;
using NetWork;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Tester_Proxy : MonoBehaviour
{
    private Proxy m_Proxy;

    public string userName = "16833336666";

    public string passWord = "111111";

    // Start is called before the first frame update
    void Start()
    {
        m_Proxy = new Proxy(userName, passWord);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            var param = new JObject();
            param.Add("userId", 97);
            m_Proxy.SendJsonRequest(INetWork.taskList.ConfigGetUrlWithParam(param), HttpMethod.Get, param);
        }
    }
}