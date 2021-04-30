using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public abstract class MonoSingleton<T> : MonoBehaviour,ISingleton where T : MonoSingleton<T>
    {
        private static T instance;
        private static bool isApplicationQuit = false;

        public static T Instance
        {
            get
            {
                if (instance == null && !isApplicationQuit)
                {
                    instance = MonoSingletonCreator.CreateMonoSingle<T>();
                }
                return instance;
            }
        }
        public virtual void InitSingleton()
        {
            
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        public static bool IApplicationQuit
        {
            get { return isApplicationQuit; }
        }

        public virtual void OnApplicationQuit()
        {
            isApplicationQuit = true;
            if (instance == null) return;
            Destroy(instance.gameObject);
            instance = null;
        }

        public virtual void OnDestroy()
        {
            instance = null;
        }
    }
}

