using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public class SingletonProperty<T> where T : class,ISingleton
    {
        private static T instance;
        private static readonly object mLock = new object();

        public static T Instace
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

        public static void Dispose()
        {
            instance = null;
        }
    }
}

