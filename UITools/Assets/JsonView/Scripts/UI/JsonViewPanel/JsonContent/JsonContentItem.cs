/****************************************************************************
 * 2021.5 BAICHUAN
 ****************************************************************************/

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace JsonView
{
    public partial class JsonContentItem : UIElement
    {
        public bool seleced;
        public List<JsonContentItem> children = new List<JsonContentItem>();
        public JToken token;
        public string path;

        private void Awake()
        {
        }

        protected override void OnBeforeDestroy()
        {
        }

        private void OnEnable()
        {
            var toggle = Toggle;
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(isOn => { ToggleContent(isOn, true); });
            var button = GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                seleced = !seleced;
                SelectContent(seleced, true);
            });
        }

        public void ToggleContent(bool isOn, bool deep = false)
        {
            Debug.Log(name + isOn);
            var jsonView = GetComponentInParent<JsonContent>();
            if (!jsonView)
            {
                return;
            }

            if (children.Count > 2)
            {
                children[0].More.gameObject.SetActive(!isOn);
            }

            for (int i = 1; i < children.Count - 1; i++)
            {
                children[i].gameObject.SetActive(isOn);
            }

            for (int i = 1; i < children.Count - 1; i++)
            {
                if (deep)
                {
                    if (children[i].Toggle.gameObject.activeSelf)
                    {
                        children[i].ToggleContent(children[i].Toggle.isOn && isOn);
                    }
                }
            }
        }

        public void SelectContent(bool select, bool deep = false)
        {
            var jsonView = GetComponentInParent<JsonContent>();
            if (!jsonView)
            {
                return;
            }

            if (deep)
            {
                // 取消选中其他项
                var items = jsonView.GetComponentsInChildren<JsonContentItem>();
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != this)
                    {
                        items[i].seleced = false;
                        items[i].SelectContent(false);
                    }
                }
            }

            //执行此项的选中
            for (int i = 0; i < children.Count; i++)
            {
                var images = children[i].gameObject.GetComponentsInChildren<Image>();
                for (int j = 0; j < images.Length; j++)
                {
                    images[j].color = select ? Color.gray : Color.white;
                }
            }

            if (deep && children.Count > 0)
            {
                jsonView.selectToken = seleced ? children[0].token : null;
                jsonView.selectStr = seleced ? children[0].path : String.Empty;
            }
        }
    }
}