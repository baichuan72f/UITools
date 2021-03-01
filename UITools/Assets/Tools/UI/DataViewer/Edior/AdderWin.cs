using System;
using UnityEditor;
using UnityEngine;

namespace Edior
{
    public class AdderWin : EditorWindow
    {
        public static AdderWin window;
        private static AdderWinImpl _impl;
        public static AdderWinImpl WinImpl
        {
            get {
                if (_impl==null)
                {
                    _impl=new AdderWinImpl();
                }

                return _impl;
            }
            set { _impl = value; }
        }


        [MenuItem("Create AdderWin/Init")]
        public static void Init()
        {
            window = (AdderWin) EditorWindow.GetWindow(typeof(AdderWin));
        }

        public void AddSender()
        {
            WinImpl.AddSender(Selection.gameObjects[0]);
        }

        public void Addshower()
        {
            WinImpl.Addshower(Selection.gameObjects[0]);
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("url");
            WinImpl.url = GUILayout.TextField(WinImpl.url);
            GUILayout.Label("time");
            WinImpl.timeStr = GUILayout.TextField(WinImpl.timeStr);
            GUILayout.Label("path");
            WinImpl.path = GUILayout.TextField(WinImpl.path);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加Sender"))
            {
                AddSender();
            }

            if (GUILayout.Button("添加shower"))
            {
                Addshower();
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    public class AdderWinImpl
    {
        public string url = "1";
        public string timeStr = "2";
        public string path = "3";

        public void AddSender(GameObject obj)
        {
            var sender = Selection.gameObjects[0].GetComponent<Sender>();
            if (sender == null)
            {
                sender = obj.AddComponent<Sender>();
            }

            sender.url = url;
            sender.time = Int32.Parse(timeStr);
        }

        public void Addshower(GameObject obj)
        {
            var shower = Selection.gameObjects[0].GetComponent<Shower>();
            if (shower == null)
            {
                shower = obj.AddComponent<Shower>();
            }

            shower.url = url;
            shower.time = Int32.Parse(timeStr);
            shower.path = path;
        }
    }
}