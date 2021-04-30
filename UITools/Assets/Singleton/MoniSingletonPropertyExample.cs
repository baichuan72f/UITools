using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public class MoniSingletonPropertyExample : MonoBehaviour,ISingleton
    {
        // Start is called before the first frame update
        private static MoniSingletonPropertyExample instance;
        private static MoniSingletonPropertyExample instance1;

        public void InitSingleton()
        {
            
        }

        void Start()
        {
            instance = MonoSingletonProperty<MoniSingletonPropertyExample>.Instance;
            instance1 = MonoSingletonProperty<MoniSingletonPropertyExample>.Instance;
            Debug.Log(instance.GetHashCode() == instance1.GetHashCode());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

