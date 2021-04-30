using System;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace UITools.Editor
{
    public class DawaWayEditlorWinow : EditorWindow
    {
        #region 数据

        //数据
        JObject FromData;
        JObject ToData;

        #endregion

        #region 布局

        Rect windowSize = new Rect(0, 0, 800, 600);

        #endregion

        #region 绘制所需

        private string fromPath;
        private string toPath;

        #endregion

        //显示窗口
        [MenuItem("GameObject/ShowDatWayWindow", false, -1)]
        public static void ShowDatWay()
        {
            var window = EditorWindow.GetWindow<DawaWayEditlorWinow>();
            window.Show();
        }

        //绘制窗体的方法
        private void OnGUI()
        {
            GUILayout.BeginArea(windowSize);
            fromPath = GUILayout.TextField(fromPath);
            if (GUILayout.Button("加载开始数据"))
            {
                FromData = LoadFile(fromPath);
            }

            toPath = GUILayout.TextField(toPath);
            if (GUILayout.Button("加载结束数据"))
            {
                ToData = LoadFile(toPath);
            }

            GUILayout.EndArea();
        }

        //读取文件数据
        JObject LoadFile(string loadPath)
        {
            using (var reader = new StreamReader(loadPath))
            {
                return JObject.Parse(reader.ReadToEnd());
            }
        }
    }
}