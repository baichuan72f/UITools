using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public class SingletonPropertyExample : MonoBehaviour
    {
        public class TestSingletonPropertyExample : ISingleton
        {
            private TestSingletonPropertyExample()
            {

            }
            public static TestSingletonPropertyExample Instance
            {
                get { return SingletonProperty<TestSingletonPropertyExample>.Instace; }
            }
            public void InitSingleton()
            {
                
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            TestSingletonPropertyExample instance1 = TestSingletonPropertyExample.Instance;
            TestSingletonPropertyExample instance2 = TestSingletonPropertyExample.Instance;
            Debug.Log(instance1.GetHashCode() == instance2.GetHashCode());

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

