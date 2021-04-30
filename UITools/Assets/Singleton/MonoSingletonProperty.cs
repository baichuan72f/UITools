using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public static class MonoSingletonProperty<T> where T:MonoBehaviour,ISingleton
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = MonoSingletonCreator.CreateMonoSingle<T>();
                }
                return instance;
            }
        }

        public static void Dispose()
        {
            if (instance)
            {
                Object.Destroy(instance.gameObject);
                instance = null;
            }
        }
    }

}
