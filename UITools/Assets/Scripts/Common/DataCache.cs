using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;

namespace Common
{
    public class DataCache
    {
        private static DataCache _instance;

        public static DataCache instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataCache();
                }

                return _instance;
            }
            set { _instance = value; }
        }


        JObject m_Cache;

        public DataCache(JObject data = null)
        {
            if (data == null)
            {
                data = new JObject();
            }

            m_Cache = data;
        }

        private string[] getPaths(string path)
        {
            return path.Split(new char['/'], StringSplitOptions.RemoveEmptyEntries);
        }

        public JToken GetValue(string path, JObject root = null)
        {
            var paths = getPaths(path);
            return GetValue(paths, root);
        }


        public JToken GetValue(string[] paths, JObject root = null)
        {
            if (paths == null || paths.Length == 0)
            {
                return null;
            }

            JToken token = root == null ? m_Cache.Root : root;
            for (int i = 0; i < paths.Length; i++)
            {
                token = token[paths[i]];
            }

            return token;
        }

        public void SetValue(string path, JToken value, JObject root = null)
        {
            var paths = getPaths(path);
            SetValue(paths, value, root);
        }

        public void SetValue(string[] paths, JToken value, JObject root = null)
        {
            //获取root
            JToken token = root == null ? m_Cache.Root : root;

            if (paths != null && paths.Length > 0)
            {
                var jObj = new JObject();
                //获取路径上的token,没有则添加
                for (int i = 0; i < paths.Length; i++)
                {
                    jObj = token.ToObject<JObject>();
                    if (!token.HasValues || !jObj.ContainsKey(paths[i]))
                    {
                        jObj.Add(paths[i], new JObject());
                    }
                    token = jObj[paths[i]];
                }
            }

            //替换value值
            token.Replace(value);
        }
    }
}