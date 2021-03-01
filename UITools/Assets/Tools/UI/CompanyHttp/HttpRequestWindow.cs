using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor;
using CompanyHttp;
using LitJson;

public class HttpRequestWindow : EditorWindow
{
    [MenuItem("自定义菜单/打开窗口")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<HttpRequestWindow>();
    }

    // 请求基本配置表  加载读取基本配置文件  登录  HTTP接口请求
    bool _tbaseConfig, _tloadConfig, _tlogin, _thttpRequest;

    private void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _tbaseConfig = EditorGUILayout.ToggleLeft("Http请求使用到的基本配置文件", _tbaseConfig);
        EditorGUILayout.EndHorizontal();
        if (_tbaseConfig)
        {
            EditorGUILayout.BeginVertical();
            drawHttpBaseConfig();
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _tloadConfig = EditorGUILayout.ToggleLeft("Http请求的基本配置文件读取", _tloadConfig);
        EditorGUILayout.EndHorizontal();
        if (_tloadConfig)
        {
            EditorGUILayout.BeginVertical();
            drawLoadHttpBaseConfig();
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _tlogin = EditorGUILayout.ToggleLeft("Http请求登录获取密钥(Token)", _tlogin);
        EditorGUILayout.EndHorizontal();
        if (_tlogin)
        {
            EditorGUILayout.BeginVertical();
            drawLogin();
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _thttpRequest = EditorGUILayout.ToggleLeft("Http接口请求和数据解析", _thttpRequest);
        EditorGUILayout.EndHorizontal();
        if (_thttpRequest)
        {
            EditorGUILayout.BeginVertical();
            drawHttpRequest();
            EditorGUILayout.EndVertical();
        }
    }

    // 请求基本配置表绘制
    string _ip = "192.168.0.1", _port = "8000", _name = "admin", _password = "admin123";
    // 配置文件生成强制性
    bool _force;
    private void drawHttpBaseConfig()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        bool b = GUILayout.Button("检查基本配置文件",GUILayout.MaxWidth(120));
        if(b)
        {
            string path = "";
            bool check = HttpRequest.CheckBaseConfigFile(ref path);
            if (check)
                EditorUtility.DisplayDialog("配置文件检查", "提示内容: \n" + "配置文件路径: " + path, "确认");
            else
                EditorUtility.DisplayDialog("配置文件检查", "提示内容: \n" + "未发现配置文件.", "确认");
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("填写配置信息,生成Http请求使用的基本配置文件.");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("请求服务IP", GUILayout.MaxWidth(100));
        _ip = EditorGUILayout.TextField(_ip, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("请求服务端口", GUILayout.MaxWidth(100));
        _port = EditorGUILayout.TextField(_port, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("登录用户名", GUILayout.MaxWidth(100));
        _name = EditorGUILayout.TextField(_name, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("登录用户密码", GUILayout.MaxWidth(100));
        _password = EditorGUILayout.TextField(_password, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        if(GUILayout.Button("生成基本配置文件", GUILayout.MaxWidth(120)))
        {
            HttpRequest.HttpBaseConfig config;
            config.address = _ip;
            config.port = _port;
            config.username = _name;
            config.password = _password;

            string fileP = HttpRequest.GenerateBaseConfigFile(config, _force);
            EditorUtility.DisplayDialog("配置文件信息", "提示内容: \n" + "配置文件路径: " + fileP, "确认");
        }
        _force = EditorGUILayout.ToggleLeft("", _force, GUILayout.MaxWidth(20));
        EditorGUILayout.LabelField("强制性: 当前检查到工程中已有配置文件,会删掉当前的生成新的配置文件.");
        EditorGUILayout.EndHorizontal();
    }


    // 加载读取基本配置文件
    private void drawLoadHttpBaseConfig()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        bool b = GUILayout.Button("读取基本配置文件", GUILayout.MaxWidth(120));
        if (b)
        {
            string info = "";
            bool load = HttpRequest.LoadBaseConfigFile(ref info);
            if (load)
                EditorUtility.DisplayDialog("配置文件读取", "提示内容: \n" + "配置文件内容: " + info, "确认");
            else
                EditorUtility.DisplayDialog("配置文件读取", "提示内容: \n" + "未能成功读取配置文件.", "确认");
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("请求地址: " + HttpRequest.Address);
        EditorGUILayout.EndHorizontal();
    }


    // 用户登录登出
    private void drawLogin()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        bool b = GUILayout.Button("用户请求登录Ex", GUILayout.MaxWidth(120));
        EditorGUILayout.LabelField("(Ex)拓展式登录: 使用的用户名和用户密码是请求基本配置文件中的信息.");
        if (b)
        {
            string info = "";
            bool load = HttpRequest.LoginEx(ref info);
            if (load)
                EditorUtility.DisplayDialog("用户登录状态", "提示内容: \n" + "登录信息: " + info, "确认");
            else
                EditorUtility.DisplayDialog("用户登录状态", "提示内容: \n" + "用户未能成功登录.", "确认");
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("登录用户名", GUILayout.MaxWidth(100));
        _name = EditorGUILayout.TextField(_name, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("登录用户密码", GUILayout.MaxWidth(100));
        _password = EditorGUILayout.TextField(_password, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        bool bb = GUILayout.Button("用户请求登录", GUILayout.MaxWidth(120));
        EditorGUILayout.LabelField("自定义登录: 使用的用户名和用户密码是来自界面的信息.");
        if (bb)
        {
            string info = "";
            bool load = HttpRequest.Login(_name, _password, ref info);
            if (load)
                EditorUtility.DisplayDialog("用户登录状态", "提示内容: \n" + "登录信息: " + info, "确认");
            else
                EditorUtility.DisplayDialog("用户登录状态", "提示内容: \n" + "用户未能成功登录.", "确认");
        }
        EditorGUILayout.EndHorizontal();
    }


    // HTTP接口请求
    string _url = "/login";
    public enum requestType { GET,POST}
    requestType _type = requestType.GET;
    bool _containP;
    string _jsonP = "{ \n    json parameter!!!\n}";
    string _kvP = "key1 = value1 = int\nkey2 = value2 = float\nkey3 = value3 = string\nkey4 = value4 = bool";
    string _resJson = "";
    string _urlName = "输入请求名称...", _pathName = "输入路径名称...";
    string _analysisPath = "path1/path2/path3/...";
    string _analysisStr = "";
    private void drawHttpRequest()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("请求地址", GUILayout.MaxWidth(80));
        _url = EditorGUILayout.TextField(_url, GUILayout.MaxWidth(360));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("请求方式", GUILayout.MaxWidth(80));
        _type = (requestType)EditorGUILayout.EnumPopup(_type, GUILayout.MaxWidth(80));

        if(GUILayout.Button("点击请求",GUILayout.MaxWidth(120)))
        {
            bool resb = false;
            if (!_containP)
            {
                if (_type == requestType.POST)
                    resb = HttpRequest.POST(_url, ref _resJson);
                else
                    resb = HttpRequest.GET(_url, ref _resJson); 
            }
            else
            {
                if (_type == requestType.POST)
                    resb = HttpRequest.POST(_url, ref _resJson, _jsonP);
                else
                {
                    Dictionary<string, object> getP = HttpRequest.HttpUrlConfig.GetGETParameter(_kvP);
                    resb = HttpRequest.GET(_url, ref _resJson, getP);
                }
            }

            if (!resb)
                EditorUtility.DisplayDialog("用户请求状态", "提示内容: \n" + "请求失败.", "确认");

            if (!string.IsNullOrEmpty(_resJson))
                _resJson = HttpRequest.FormatJsonStr(_resJson);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("包含参数", GUILayout.MaxWidth(80));
        _containP = EditorGUILayout.ToggleLeft("", _containP, GUILayout.MaxWidth(160));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (_containP)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(16);
            if (_type == requestType.POST)
                _jsonP = EditorGUILayout.TextArea(_jsonP, GUILayout.MaxWidth(200), GUILayout.MinHeight(260));
            else
                _kvP = EditorGUILayout.TextArea(_kvP, GUILayout.MaxWidth(200), GUILayout.MinHeight(260));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.TextArea(_resJson, GUILayout.MaxWidth(200), GUILayout.MinHeight(260));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _urlName = EditorGUILayout.TextField(_urlName, GUILayout.MaxWidth(200));
        if(GUILayout.Button("存储这条请求", GUILayout.MaxWidth(100)))
        {
            if (string.IsNullOrEmpty(_urlName))
                EditorUtility.DisplayDialog("请求信息存储", "提示内容: \n" + "存储信息失败,请求名称不能为空.", "确认");
            else
            {
                string filePath = "";
                if (!HttpRequest.CheckUrlsConfigFile(ref filePath))
                {
                    System.IO.File.WriteAllText(filePath, "", System.Text.Encoding.UTF8);
                    AssetDatabase.Refresh();
                }

                HttpRequest.HttpRequestUrlsConfig config = null;
                string str = System.IO.File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(str))
                    config = new HttpRequest.HttpRequestUrlsConfig();
                else
                    config = LitJson.JsonMapper.ToObject<HttpRequest.HttpRequestUrlsConfig>(str);

                List<HttpRequest.HttpUrlConfig> httpUrls = new List<HttpRequest.HttpUrlConfig>();
                if (config.httpUrls != null && config.httpUrls.Length > 0)
                    httpUrls.AddRange(config.httpUrls);

                if(httpUrls.Count>0&&httpUrls.TrueForAll(p=>p.requestName.Equals(_urlName.Trim())))
                    EditorUtility.DisplayDialog("请求信息存储", "提示内容: \n" + "存储信息失败,配置文件中已有对应的请求名称.", "确认");
                else
                {
                    HttpRequest.HttpUrlConfig http = new HttpRequest.HttpUrlConfig(
                        _urlName.Trim(), _url.Trim(), _type == requestType.GET, _containP ? (_type == requestType.GET ? _kvP : _jsonP) : "");
                    httpUrls.Add(http);

                    config.httpUrls = httpUrls.ToArray();
                    HttpRequest.GenerateHttpUrlsConfigFile(config);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        EditorGUILayout.LabelField("下面填写取值路径测试提取所需要的值.", GUILayout.MaxWidth(200));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _analysisPath = EditorGUILayout.TextField(_analysisPath, GUILayout.MaxWidth(200));
        if (GUILayout.Button("点击解析数据", GUILayout.MaxWidth(100)))
        {
            if(string.IsNullOrEmpty(_resJson)||string.IsNullOrEmpty(_analysisPath))
                EditorUtility.DisplayDialog("解析数据", "提示内容: \n" + "解析数据失败,解析的源Json内容或者解析路径信息为Empty.", "确认");
            else
            {
                string[] ps = _analysisPath.Split('/');
                JsonData jsonData = JsonMapper.ToObject(_resJson);

                foreach (string p in ps)
                {
                    if (string.IsNullOrEmpty(p))
                        continue;

                    if (jsonData.IsArray)
                        jsonData = jsonData[int.Parse(p)];
                    else
                        jsonData = jsonData[p];
                }

                _analysisStr = jsonData.ToString();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _analysisStr = EditorGUILayout.TextArea(_analysisStr, GUILayout.MaxWidth(200), GUILayout.MaxHeight(160));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(16);
        _pathName = EditorGUILayout.TextField(_pathName, GUILayout.MaxWidth(200));
        if (GUILayout.Button("存储这条路径", GUILayout.MaxWidth(100)))
        {
            if (string.IsNullOrEmpty(_pathName))
                EditorUtility.DisplayDialog("解析路径存储", "提示内容: \n" + "存储信息失败,路径名称不能为空.", "确认");
            else
            {
                string filePath = "";
                if (!HttpRequest.CheckPathsConfigFile(ref filePath))
                {
                    System.IO.File.WriteAllText(filePath, "", System.Text.Encoding.UTF8);
                    AssetDatabase.Refresh();
                }

                HttpRequest.DataAnalysisPathsConfig config = null;
                string str = System.IO.File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(str))
                    config = new HttpRequest.DataAnalysisPathsConfig();
                else
                    config = LitJson.JsonMapper.ToObject<HttpRequest.DataAnalysisPathsConfig>(str);

                List<HttpRequest.DataAnalysisPathConfig> httpPaths = new List<HttpRequest.DataAnalysisPathConfig>();
                if (config.dataPaths != null && config.dataPaths.Length > 0)
                    httpPaths.AddRange(config.dataPaths);

                if (httpPaths.Count > 0 && httpPaths.Any(p => p.analysisName.Equals(_pathName.Trim())))
                    EditorUtility.DisplayDialog("解析路径存储", "提示内容: \n" + "存储路径失败,配置文件中已有对应的路径名称.", "确认");
                else
                {
                    HttpRequest.DataAnalysisPathConfig path = new HttpRequest.DataAnalysisPathConfig(
                        _pathName.Trim(), _urlName.Trim(), _analysisPath.Trim());
                    httpPaths.Add(path);

                    config.dataPaths = httpPaths.ToArray();
                    HttpRequest.GenerateHttpPathsConfigFile(config);
                    EditorUtility.DisplayDialog("提示", "提示内容: \n" + "存储路径成功.", "确认");
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
