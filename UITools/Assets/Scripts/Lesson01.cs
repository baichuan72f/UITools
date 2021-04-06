using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson01 : MonoBehaviour
{
    private DataProxy m_DataProxy;

    // Start is called before the first frame update
    void Start()
    {
        m_DataProxy = new DataProxy("15888888888", "123456");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
        }
    }
}