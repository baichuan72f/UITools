using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HFrameExample
{
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class,ISingleton
        {
            ///获取私有的构造函数
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            // 获取无参构造函数
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Public Constructor() not found! in " + typeof(T));
            }

            var instance = ctor.Invoke(null) as T;
            instance.InitSingleton();
            return instance;
        }
    }
}

