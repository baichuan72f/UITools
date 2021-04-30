using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public class MonoSingletonExample : MonoBehaviour
    {

        public class MonoSingletonTest : MonoSingleton<MonoSingletonTest>
        {
            public void MonoSingletonTestLog()
            {
                Debug.Log("MonoSingletonTest");
            }
        }

        void Start()
        {
            MonoSingletonTest.Instance.MonoSingletonTestLog();

            Debug.Log(MonoSingletonTest.Instance == MonoSingletonTest.Instance);
        }
        void Update()
        {

        }
    }
}

