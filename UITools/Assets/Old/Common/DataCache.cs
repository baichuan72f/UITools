using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using QF.Extensions;
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

        private JObject m_JsonRoot = new JObject();

        public void SetValue(string key, string value)
        {
            if (!cache.ContainsKey(key)) cache.Add(key, value);
            cache[key] = value;
        }

        public Dictionary<string, string> cache = new Dictionary<string, string>();

        public string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key) || !cache.ContainsKey(key))
            {
                return string.Empty;
            }

            return cache[key];
        }

        private string[] getPaths(string path)
        {
            return path.Split(new char['/'], StringSplitOptions.RemoveEmptyEntries);
        }

        public JToken GetJsonValue(string path, JObject root = null)
        {
            var paths = getPaths(path);
            return GetJsonValue(paths, root);
        }


        public JToken GetJsonValue(string[] paths, JObject root = null)
        {
            if (paths == null || paths.Length == 0)
            {
                return null;
            }

            JToken token = root == null ? m_JsonRoot : root;
            for (int i = 0; i < paths.Length; i++)
            {
                token = token[paths[i]];
            }

            return token;
        }


        public void SetJsonValue(string path, JToken value, JObject root = null)
        {
            var paths = getPaths(path);
            SetJsonValue(paths, value, root);
        }

        public void SetJsonValue(string[] paths, JToken value, JObject root = null)
        {
            //获取root
            JToken token = root == null ? m_JsonRoot : root;

            if (paths != null && paths.Length > 0)
            {
                var jObj = new JObject();
                //获取路径上的token,没有则添加
                for (int i = 0; i < paths.Length; i++)
                {
                    jObj = token.ToObject<JObject>();
                    if (!token.HasValues || !jObj.JsonDataContainsKey(paths[i]))
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