/****************************************************************************
 * 2021.5 BAICHUAN
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using QF.Extensions;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.Serialization;

namespace JsonView
{
    public partial class JsonContent : UIElement
    {
        public JToken selectToken;

        public string selectStr;
        //int index;
        private JsonItem currentItem = JsonItem.Next;
        private int currentIndex;
        private JsonItem[][] inLineArr;
        public string path = "/JsonView/Content.json";

        private void Awake()
        {
            var loadPath = Application.streamingAssetsPath + path;
            using (var reader = new StreamReader(loadPath))
            {
                var str = reader.ReadToEnd();
                var obj = JObject.Parse(str);
                ShowData(0, obj, null,true);
            }
        }

        protected override void OnBeforeDestroy()
        {
        }

        void Init()
        {
            currentItem = JsonItem.Next;
            var line1 = new JsonItem[]
                {JsonItem.Key, JsonItem.Perporty, JsonItem.Value, JsonItem.Next}; //"key1": "value",
            var line2 = new JsonItem[]
            {
                JsonItem.Key, JsonItem.Perporty, JsonItem.ObjectStart, JsonItem.ObjectEnd, JsonItem.Next
            }; //"key1": {},
            var line3 = new JsonItem[]
            {
                JsonItem.Key, JsonItem.Perporty, JsonItem.ArrayStart, JsonItem.ObjectEnd, JsonItem.Next
            }; //"key1": [],
            inLineArr = new[] {line1, line2, line3};
            currentIndex = 0;
            var objs = GetComponentsInChildren<JsonContentItem>(true);
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].gameObject.SetActive(false);
                for (int j = 0; j < objs[i].ItemContent.transform.childCount; j++)
                {
                    objs[i].ItemContent.transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }

        public void ShowData(int deep, JToken target, string[] paths = null, bool isNew = false)
        {
            if (isNew || deep == 0)
            {
                Init();
            }

            if (target.IsNull())
            {
                return;
            }

            if (paths == null)
            {
                paths = new string[0];
            }

            if (target.IsObject())
            {
                var startItem = writeItem(deep, JsonItem.ObjectStart);
                if (target != null)
                {
                    int idx = 0;
                    var source = target.ToObject<JObject>();
                    foreach (var sourceItem in source)
                    {
                        if (idx > 0)
                        {
                            writeItem(deep, JsonItem.Next);
                        }

                        idx++;
                        writeItem(deep + 1, JsonItem.Key, sourceItem.Key);
                        writeItem(deep + 1, JsonItem.Perporty);
                        var p = new List<string>(paths);
                        p.Add(sourceItem.Key);
                        ShowData(deep + 1, sourceItem.Value,p.ToArray());
                    }
                }

                var endItem = writeItem(deep, JsonItem.ObjectEnd);
                var viewItems = GetComponentsInChildren<JsonContentItem>(true);
                var startIndex = Array.IndexOf(viewItems, startItem);
                var endIndex = Array.IndexOf(viewItems, endItem);
                startItem.children.Clear();
                endItem.children.Clear();
                viewItems.Where((v, i) => startIndex <= i && i <= endIndex).ForEach(v =>
                {
                    startItem.children.Add(v);
                    endItem.children.Add(v);
                });
                startItem.token = endItem.token = target;
                startItem.path = endItem.path = string.Join("/", paths);
                return;
            }

            if (target.IsArray())
            {
                var startItem = writeItem(deep, JsonItem.ArrayStart);
                if (target != null)
                {
                    var source = target.ToArray();
                    for (int i = 0; i < source.Length; i++)
                    {
                        if (i > 0)
                        {
                            writeItem(deep, JsonItem.Next);
                        }
                        var p = new List<string>(paths);
                        p.Add(i.ToString());
                        ShowData(deep + 1, source[i]?.ToObject<JObject>(),p.ToArray());
                    }
                }

                var endItem = writeItem(deep, JsonItem.ArrayEnd);
                var viewItems = GetComponentsInChildren<JsonContentItem>(true);
                var startIndex = Array.IndexOf(viewItems, startItem);
                var endIndex = Array.IndexOf(viewItems, endItem);
                startItem.children.Clear();
                endItem.children.Clear();
                viewItems.Where((v, i) => startIndex <= i && i <= endIndex).ForEach(v =>
                {
                    startItem.children.Add(v);
                    endItem.children.Add(v);
                });
                startItem.token = endItem.token = target;
                startItem.path = endItem.path = string.Join("/", paths);
                return;
            }

            var keyValueItem = writeItem(deep, JsonItem.Value, target.ToString());
            keyValueItem.children.Clear();
            keyValueItem.children.Add(keyValueItem);
            keyValueItem.token = target;
            keyValueItem.path = string.Join("/", paths);
        }

        public JsonContentItem writeItem(int deep, JsonItem itemType, string itemConternt = null)
        {
            bool inline = false;
            for (int i = 0; i < inLineArr.Length; i++)
            {
                var idx1 = Array.IndexOf(inLineArr[i], itemType);
                var idx2 = Array.IndexOf(inLineArr[i], currentItem);
                inline = idx1 != -1 && idx2 != -1 && idx1 > idx2;
                if (inline)
                {
                    break;
                }
            }

            currentItem = itemType;
            if (!inline)
            {
                currentIndex++;
            }

            var currentViewItem = currentIndex < transform.childCount
                ? transform.GetChild(currentIndex).gameObject
                : Instantiate<GameObject>(jsonContentItem.gameObject, jsonContentItem.transform.parent);
            currentViewItem.SetActive(true);
            var viewItew = currentViewItem.GetComponent<JsonContentItem>();
            if (!inline)
            {
                viewItew.Space.rectTransform.SetSizeWidth(20 * deep);
                viewItew.Space.gameObject.SetActive(true);
            }

            //Debug.Log(itemType.ToString());
            var obj = viewItew.Space.gameObject;
            switch (itemType)
            {
                case JsonItem.Toggle:
                    obj = viewItew.Toggle.gameObject;
                    break;
                case JsonItem.Sapce:
                    obj = viewItew.Space.gameObject;
                    break;
                case JsonItem.ObjectStart:
                    obj = viewItew.ObjectStart.gameObject;
                    viewItew.Toggle.gameObject.SetActive(true);
                    break;
                case JsonItem.ObjectEnd:
                    obj = viewItew.ObjectEnd.gameObject;
                    break;
                case JsonItem.ArrayStart:
                    obj = viewItew.ArrayStart.gameObject;
                    viewItew.Toggle.gameObject.SetActive(true);
                    break;
                case JsonItem.ArrayEnd:
                    obj = viewItew.ArrayEnd.gameObject;
                    break;
                case JsonItem.Key:
                    obj = viewItew.Key.gameObject;
                    viewItew.Key.text = itemConternt;
                    break;
                case JsonItem.Value:
                    obj = viewItew.Value.gameObject;
                    viewItew.Value.text = itemConternt;
                    break;
                case JsonItem.Perporty:
                    obj = viewItew.Perporty.gameObject;
                    break;
                case JsonItem.Next:
                    obj = viewItew.Next.gameObject;
                    break;
            }

            obj.SetActive(true);
            return viewItew;
        }
    }

    public enum JsonItem
    {
        Toggle,
        Sapce,
        ObjectStart,
        ObjectEnd,
        ArrayStart,
        ArrayEnd,
        Key,
        Value,
        Perporty,
        Next
    }
}