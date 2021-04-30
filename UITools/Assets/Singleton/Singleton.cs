using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T instance;
        static object mLock = new object();

        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    if (instance == null)
                    {
                        instance = SingletonCreator.CreateSingleton<T>();
                    }
                    return instance;
                }
            }
        }

        public virtual void Dispose()
        {
            instance = null;
        }

        public void InitSingleton()
        {
            
        }
    }
}

