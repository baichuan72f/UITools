using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CompanyHttp
{
    public class RequestSender : MonoBehaviour
    {
        public SendTime sendTime;

        private void Awake()
        {
            SendRequest(SendTime.Awake);
        }

        private void Start()
        {
            SendRequest(SendTime.Start);
        }

        private void Update()
        {
            //SendRequest(SendTime.Update);
        }

        private void Reset()
        {
            SendRequest(SendTime.Reset);
        }

        private void OnEnable()
        {
            SendRequest(SendTime.OnEnable);
        }

        private void OnDisable()
        {
            SendRequest(SendTime.OnDisable);
        }

        private void OnDestroy()
        {
            SendRequest(SendTime.OnDestroy);
        }

        public void SendRequest()
        {
            SendRequest(SendTime.Now);
        }
        public void SendRequest(SendTime time)
        {
            switch (time)
            {
                case SendTime.None:
                    Debug.Log("return");
                    break;
                case SendTime.Now:
                    Debug.Log(time);
                    break;
                default:
                    if (time == sendTime) Debug.Log(time);
                    break;
            }
        }
    }

    public enum SendTime
    {
        None,
        Now,
        Awake,
        Start,
        Reset,
        OnEnable,
        OnDisable,
        OnDestroy
    }
}