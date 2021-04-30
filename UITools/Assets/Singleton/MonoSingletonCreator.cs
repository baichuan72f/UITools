using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public static class MonoSingletonCreator
    {
        public static T CreateMonoSingle<T>() where T : MonoBehaviour, ISingleton
        {
            var instance = Object.FindObjectOfType<T>();

            if (instance)
            {
                instance.InitSingleton();
                return instance;
            }

            GameObject obj = new GameObject(typeof(T).Name);
            Object.DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
            instance.InitSingleton();
            return instance;
        }
    }
}

