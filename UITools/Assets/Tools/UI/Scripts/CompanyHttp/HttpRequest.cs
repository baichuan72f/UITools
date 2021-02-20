using LitJson;
using RestSharp;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;


/* CompanyHttp:WWHY公司后台Http请求工具 */
namespace CompanyHttp
{
    /// <summary>
    /// 和公司服务端请求通信工具
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Http请求基本配置信息
        /// 包含内容：
        /// 请求服务地址：Address
        /// 请求服务端口：Port
        /// 登录用户名：  Username
        /// 用户密码：    Password
        /// </summary>
        public struct HttpBaseConfig
        {
            /// <summary>/// 请求服务地址/// </summary>
            public string address;

            /// <summary>/// 请求服务端口/// </summary>
            public string port;

            /// <summary>/// 登录用户名/// </summary>
            public string username;

            /// <summary>/// 用户密码/// </summary>
            public string password;


            public HttpBaseConfig(string address, string port, string username, string password)
            {
                this.address = address;
                this.port = port;
                this.username = username;
                this.password = password;
            }
        }

        /// <summary>
        /// 一条请求配置信息
        /// </summary>
        public struct HttpUrlConfig
        {
            /// <summary>/// 请求名称/// </summary>
            public string requestName;

            /// <summary>/// 请求地址/// </summary>
            public string requestUrl;

            /// <summary>/// 请求参数/// </summary>
            public string requestParameter;

            /// <summary>/// 请求是GET方式吗?/// </summary>
            public bool requestGET;


            /// <summary>
            /// 请求是否包含参数
            /// </summary>
            public bool ContainParameter()
            {
                return !string.IsNullOrEmpty(requestParameter);
            }


            /// <summary>
            /// 解析GET的键值对参数
            /// </summary>
            /// <param name="getParameter">参数</param>
            /// <returns>count>=0</returns>
            public static Dictionary<string, object> GetGETParameter(string getParameter)
            {
                Dictionary<string, object> dicP = new Dictionary<string, object>();
                if (string.IsNullOrEmpty(getParameter))
                    return dicP;

                // 类型只有 int / float / string / bool
                string[] lines = getParameter.Split('\n');
                if (lines != null && lines.Length > 0)
                {
                    string key = "";
                    object value = null;
                    foreach (string line in lines)
                    {
                        string[] p3 = line.Split('=');
                        if (p3 == null || p3.Length != 3)
                            continue;

                        key = p3[0].Trim();
                        string vt = p3[2].Trim();
                        if (vt == "int")
                            value = Convert.ToInt32(p3[1].Trim());
                        else if (vt == "float")
                            value = Convert.ToSingle(p3[1].Trim());
                        else if (vt == "bool")
                            value = Convert.ToBoolean(p3[1].Trim());
                        else
                            value = p3[1].Trim();

                        dicP.Add(key, value);
                    }
                }

                return dicP;
            }


            public HttpUrlConfig(string name, string url, bool get, string param)
            {
                requestName = name;
                requestUrl = url;
                requestParameter = param;
                requestGET = get;
            }
        }

        /// <summary>
        /// Json数据解析路径配置
        /// </summary>
        public struct DataAnalysisPathConfig
        {
            public string analysisName;

            public string requestName;

            public string analysisPath;


            public DataAnalysisPathConfig(string name, string requestName, string path)
            {
                analysisName = name;
                this.requestName = requestName;
                analysisPath = path;
            }
            /// <summary>
            /// 按照analysisPath解析jsonData
            /// </summary>
            /// <param name="jsonData"></param>
            /// <returns></returns>
            public JsonData[,] GetAnalysisArray(JsonData jsonData,string analysisPath)
            {
                if (jsonData == null || string.IsNullOrEmpty(analysisPath))
                    return null;
                JsonData[,] dataArr = new JsonData[1, 1];
                dataArr[0, 0] = jsonData;
                string[] ps = analysisPath.Split(new Char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string p in ps)
                {
                    var len = GetOneAnalysis(dataArr[0, 0], p).Length;
                    JsonData[] resultArr = null;
                    if (dataArr.GetLength(0) == 1)
                    {
                        dataArr = new JsonData[len, 1];
                        for (int i = 0; i < dataArr.GetLength(0); i++)
                        {
                            for (int j = 0; j < dataArr.GetLength(1); j++)
                            {
                                if (j == 0) resultArr = GetOneAnalysis(dataArr[i, j], p);
                                dataArr[i, j] = resultArr[i];
                            }
                        }
                    }
                    else if ((dataArr.GetLength(0) == 1))
                    {
                        dataArr = new JsonData[dataArr.GetLength(0), len];
                        for (int i = 0; i < dataArr.GetLength(0); i++)
                        {
                            for (int j = 0; j < dataArr.GetLength(1); j++)
                            {
                                if (i == 0) resultArr = GetOneAnalysis(dataArr[i, j], p);
                                dataArr[i, j] = resultArr[j];
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dataArr.GetLength(0); i++)
                        {
                            for (int j = 0; j < dataArr.GetLength(1); j++)
                            {
                                resultArr = GetOneAnalysis(dataArr[i, j], p);
                                dataArr[i, j] = resultArr[0];
                            }
                        }
                    }
                }
                return dataArr;
            }
            /// <summary>
            /// 按照pathStr解析单层jsonData
            /// </summary>
            /// <param name="jsonData"></param>
            /// <param name="pathStr"></param>
            /// <returns></returns>
            public JsonData[] GetOneAnalysis(JsonData jsonData, string pathStr)
            {
                var result = new JsonData[0];
                var idx = 0;
                if (!jsonData.IsArray)
                {
                    result = new JsonData[] {jsonData[pathStr]};
                }
                else if (int.TryParse(pathStr, out idx))
                {
                    result = new JsonData[] {jsonData[idx]};
                }
                else
                {
                    result = new JsonData[jsonData.ValueAsArray().Count];
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = jsonData.ValueAsArray()[i];
                    }
                }
                return result;
            }

            public string GetOneAnalysis(string jsonData)
            {
                if (string.IsNullOrEmpty(jsonData))
                    return "";

                return GetOneAnalysis(JsonMapper.ToJson(jsonData));
            }
        }


        /// <summary>
        /// 请求地址Json
        /// </summary>
        public class HttpRequestUrlsConfig
        {
            public HttpUrlConfig[] httpUrls;
        }


        /// <summary>
        /// 数据解析路径
        /// </summary>
        public class DataAnalysisPathsConfig
        {
            public DataAnalysisPathConfig[] dataPaths;
        }


        /// <summary>/// 基本配置文件名称/// </summary>
        public const string HttpBaseConfigFileName = "HttpBaseConfig.json";

        /// <summary>/// 请求路径配置文件名称/// </summary>
        public const string HttpUrlsConfigFileName = "HttpUrlsConfig.json";

        /// <summary>/// 数据解析路径配置文件名称/// </summary>
        public const string HttpAnalysisPathsConfigFileName = "HttpAnalysisPathsConfig.json";

        /// <summary>/// 请求配置文件子目录/// </summary>
        public const string SubDirectory = "CompanyHttp";


        /// <summary>/// 请求地址/// </summary>
        public static string Address = string.Empty; // "http://60.2.7.228:6009";

        /// <summary>/// 登录后的Token/// </summary>
        public static string LoginToken = string.Empty;

        /// <summary>/// 请求失败后，自动再次尝试的次数/// </summary>
        public static int TryMaxCount = 3;

        // 当前尝试次数
        private static int _tryCount = 0;

        // 配置文件中的用户名和用户密码
        private static string _username, _password;


        /// <summary>
        /// 生成Http的基本配置文件
        /// </summary>
        /// <param name="config">配置文件的配置信息</param>
        /// <param name="force">true 强制生成(如果目录下已有基本配置文件，则删除重新生成)</param>
        /// <returns>生成的文件放置的完整路径</returns>
        public static string GenerateBaseConfigFile(HttpBaseConfig config, bool force = false)
        {
            // 1.整理配置文件放置目录
            string dic = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(SubDirectory))
                dic = dic + "/" + SubDirectory;

            // 2.检测并生成目录
            if (!Directory.Exists(dic)) // * Asset资源目录刷新
            {
                Directory.CreateDirectory(dic);
                Debug.LogWarning("已创建生成目录: " + dic);
                UnityEditor.AssetDatabase.Refresh();
            }

            // 3.检查配置文件和强制性
            string filePath = dic + "/" + HttpBaseConfigFileName;
            if (File.Exists(filePath))
            {
                if (!force) // * 存在配置文件非强制性时
                {
                    Debug.LogWarning("已存在配置文件,但非强制性生成...");
                    return filePath;
                }
            }

            // 4.配置Json基本信息，并生成配置文件
            //HttpBaseConfig config;
            //config.address = "192.168.0.1";
            //config.port = "8000";
            //config.username = "admin";
            //config.password = "abc123456";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            LitJson.JsonWriter jw = new LitJson.JsonWriter(sb);

            // 进行写入到文件中时保持json显示格式
            jw.PrettyPrint = true; // * 启用美观的打印
            jw.IndentValue = 4; // * 缩进空格数量

            JsonMapper.ToJson(config, jw);
            File.WriteAllText(filePath, sb.ToString(), System.Text.Encoding.UTF8);
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("已生成配置文件: " + filePath);

            // 5.返回生成配置文件路径信息
            return filePath;
        }


        /// <summary>
        /// 生成HttpUrls的配置文件
        /// </summary>
        /// <param name="config">配置文件内容</param>
        /// <param name="force">true 强制生成(如果目录下已有基本配置文件，则删除重新生成)</param>
        /// <returns>生成的文件放置的完整路径</returns>
        public static string GenerateHttpUrlsConfigFile(HttpRequestUrlsConfig config, bool force = true)
        {
            // 1.整理配置文件放置目录
            string dic = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(SubDirectory))
                dic = dic + "/" + SubDirectory;

            // 2.检测并生成目录
            if (!Directory.Exists(dic)) // * Asset资源目录刷新
            {
                Directory.CreateDirectory(dic);
                Debug.LogWarning("已创建生成目录: " + dic);
                UnityEditor.AssetDatabase.Refresh();
            }

            // 3.检查配置文件和强制性
            string filePath = dic + "/" + HttpUrlsConfigFileName;
            if (File.Exists(filePath))
            {
                if (!force) // * 存在配置文件非强制性时
                {
                    Debug.LogWarning("已存在配置文件,但非强制性生成...");
                    return filePath;
                }
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            LitJson.JsonWriter jw = new LitJson.JsonWriter(sb);

            // 进行写入到文件中时保持json显示格式
            jw.PrettyPrint = true; // * 启用美观的打印
            jw.IndentValue = 4; // * 缩进空格数量

            JsonMapper.ToJson(config, jw);
            File.WriteAllText(filePath, sb.ToString(), System.Text.Encoding.UTF8);
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("已生成配置文件: " + filePath);

            // 5.返回生成配置文件路径信息
            return filePath;
        }


        /// <summary>
        /// 生成Http数据解析的Paths的配置文件
        /// </summary>
        /// <param name="config">配置文件内容</param>
        /// <param name="force">true 强制生成(如果目录下已有基本配置文件，则删除重新生成)</param>
        /// <returns>生成的文件放置的完整路径</returns>
        public static string GenerateHttpPathsConfigFile(DataAnalysisPathsConfig config, bool force = true)
        {
            // 1.整理配置文件放置目录
            string dic = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(SubDirectory))
                dic = dic + "/" + SubDirectory;

            // 2.检测并生成目录
            if (!Directory.Exists(dic)) // * Asset资源目录刷新
            {
                Directory.CreateDirectory(dic);
                Debug.LogWarning("已创建生成目录: " + dic);
                UnityEditor.AssetDatabase.Refresh();
            }

            // 3.检查配置文件和强制性
            string filePath = dic + "/" + HttpAnalysisPathsConfigFileName;
            if (File.Exists(filePath))
            {
                if (!force) // * 存在配置文件非强制性时
                {
                    Debug.LogWarning("已存在配置文件,但非强制性生成...");
                    return filePath;
                }
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            LitJson.JsonWriter jw = new LitJson.JsonWriter(sb);

            // 进行写入到文件中时保持json显示格式
            jw.PrettyPrint = true; // * 启用美观的打印
            jw.IndentValue = 4; // * 缩进空格数量

            JsonMapper.ToJson(config, jw);
            File.WriteAllText(filePath, sb.ToString(), System.Text.Encoding.UTF8);
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("已生成配置文件: " + filePath);

            // 5.返回生成配置文件路径信息
            return filePath;
        }


        /// <summary>
        /// 反序列话请求地址配置文件
        /// </summary>
        /// <returns>null/实例</returns>
        public static HttpRequestUrlsConfig GetUrlsConfig()
        {
            string filePath = "";
            if (!CheckUrlsConfigFile(ref filePath))
                return null;

            string str = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(str))
                return null;

            return JsonMapper.ToObject<HttpRequestUrlsConfig>(str);
        }


        /// <summary>
        /// 反序列话请求解析路径配置文件
        /// </summary>
        /// <returns>null/实例</returns>
        public static DataAnalysisPathsConfig GetPathsConfig()
        {
            string filePath = "";
            if (!CheckPathsConfigFile(ref filePath))
                return null;

            string str = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(str))
                return null;

            return JsonMapper.ToObject<DataAnalysisPathsConfig>(str);
        }


        /// <summary>
        /// 生成Http的基本配置文件
        /// config没有赋值,是基本信息样式
        /// </summary>
        /// <param name="force">true 强制生成(如果目录下已有基本配置文件，则删除重新生成)</param>
        /// <returns>生成的文件放置的完整路径</returns>
        public static string GenerateBaseConfigFile(bool force = false)
        {
            // 1.配置Json基本信息，并生成配置文件
            HttpBaseConfig config;
            config.address = "192.168.0.1";
            config.port = "8000";
            config.username = "admin";
            config.password = "abc123456";

            // 2.生成配置文件
            return GenerateBaseConfigFile(config, force);
        }


        /// <summary>
        /// 读取Http的基本配置文件的信息
        /// </summary>
        /// <param name="info">读取到的配置信息</param>
        /// <returns>true 成功读取</returns>
        public static bool LoadBaseConfigFile(ref string info)
        {
            string filePath = "";
            // 1.检查基本配置文件
            if (!CheckBaseConfigFile(ref filePath))
            {
                Debug.LogWarning("读取Http基本信息失败！没有发现<HttpBaseConfig.json>配置文件。");
                return false;
            }

            // 2.文件读取出配置信息并序列化信息
            info = File.ReadAllText(filePath);
            HttpBaseConfig config = JsonMapper.ToObject<HttpBaseConfig>(info);

            // 3.进行信息整理
            Address = string.Format("http://{0}:{1}", config.address, config.port);
            _username = config.username;
            _password = config.password;

            return true;
        }


        /// <summary>
        /// 登录
        /// 目的是拿到有效密钥: Token
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="jsonData">请求返回的json格式信息</param>
        /// <param name="loginUrl">登录请求地址</param>
        /// <returns>true/false 请求成功/失败</returns>
        public static bool Login(string username, string password, ref string jsonData, string loginUrl = "/login")
        {
            // 1.登录请求
            //string jsonParameter = "{\r\n    \"agent\":\"string\",\r\n    \"code\":\"string\",\r\n    \"password\":\"" + password +
            //    "\",\r\n    \"phonenumber\":\"" + username + "\",\r\n    \"uuid\":\"string\"\r\n}";
            string jsonParameter = "{ \"password\":\"" + password + "\",\"phonenumber\":\"" + username + "\" }";
            bool res = POST(loginUrl, ref jsonData, jsonParameter, false);

            // 2.检查登录结果
            if (res)
            {
                Debug.Log("登录请求成功！");
                JsonData data = JsonMapper.ToObject(jsonData);
                LoginToken = data["token"].ToString();
                Debug.Log("成功登录Token: " + LoginToken);
            }
            else
                Debug.Log("登录请求失败！");

            return res;
        }


        /// <summary>
        /// 登录拓展(使用的是配置中的用户名和密码)
        /// 目的是拿到有效密钥: Token
        /// </summary>
        /// <param name="jsonData">请求返回的json格式信息</param>
        /// <param name="loginUrl">登录请求地址</param>
        /// <returns>true/false 请求成功/失败</returns>
        public static bool LoginEx(ref string jsonData, string loginUrl = "/login")
        {
            // 1.检查配置解析出来的用户名和密码
            if (string.IsNullOrEmpty(_username) && string.IsNullOrEmpty(_password))
                return false;

            // 2.使用配置的用户名和密码进行登录
            return Login(_username, _password, ref jsonData, loginUrl);
        }


        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="jsonData">请求返回的json格式信息</param>
        /// <param name="logoutUrl">登出请求地址</param>
        /// <returns>true/false 请求成功/失败</returns>
        public static bool Logout(ref string jsonData, string logoutUrl = "/loginOut")
        {
            // 1.登出请求
            bool res = POST(logoutUrl, ref jsonData, null, true);

            // 2.检查登录结果
            Debug.Log(res ? "登出请求成功！" : "登出请求失败！");
            if (res)
                LoginToken = "";

            return res;
        }


        /// <summary>
        /// 检查基本配置文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>true 路径下文件已存在</returns>
        public static bool CheckBaseConfigFile(ref string filePath)
        {
            // 1. 拼接文件路径
            string dic = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(SubDirectory))
                dic = dic + "/" + SubDirectory;
            string _filePath = dic + "/" + HttpBaseConfigFileName;
            filePath = _filePath;

            // 2. 检查文件
            return File.Exists(_filePath);
        }


        /// <summary>
        /// 检查请求路径配置文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>true 路径下文件已存在</returns>
        public static bool CheckUrlsConfigFile(ref string filePath)
        {
            // 1. 拼接文件路径
            string dic = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(SubDirectory))
                dic = dic + "/" + SubDirectory;
            string _filePath = dic + "/" + HttpUrlsConfigFileName;
            filePath = _filePath;

            // 2. 检查文件
            return File.Exists(_filePath);
        }


        /// <summary>
        /// 检查解析路径配置文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>true 路径下文件已存在</returns>
        public static bool CheckPathsConfigFile(ref string filePath)
        {
            // 1. 拼接文件路径
            string dic = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(SubDirectory))
                dic = dic + "/" + SubDirectory;
            string _filePath = dic + "/" + HttpAnalysisPathsConfigFileName;
            filePath = _filePath;

            // 2. 检查文件
            return File.Exists(_filePath);
        }


        /// <summary>
        /// GET请求
        /// 请求使用的参数协议: application/x-www-form-urlencoded
        /// </summary>
        /// <param name="address">请求地址 例如: "/login"</param>
        /// <param name="jsonData">请求返回的json格式信息</param>
        /// <param name="parameter">请求使用到的参数</param>
        /// <returns>true/false 请求成功/失败</returns>
        public static bool GET(string address, ref string jsonData, Dictionary<string, object> parameter = null)
        {
            // 1.检查服务地址
            if (string.IsNullOrEmpty(Address))
            {
                Debug.LogWarning("GET请求失败！请求服务地址为空<Address is empty>。");
                return false;
            }

            // 2.检查密钥Token
            if (string.IsNullOrEmpty(LoginToken))
            {
                Debug.LogWarning("GET请求失败！请求密钥为空<Token is empty>。");
                return false;
            }

            // 3.检查请求地址
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogWarning("GET请求失败！请求地址为空<address is empty>。");
                return false;
            }

            // 4.检查已自动尝试的次数
            if (_tryCount >= TryMaxCount)
            {
                Debug.LogWarning("GET请求失败！自动尝试请求次数已达最大值。");
                _tryCount = 0;
                return false;
            }

            // 5.POST请求
            var client = new RestClient(Address + address);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("token", LoginToken);
            // 添加请求参数
            if (parameter != null && parameter.Count > 0)
            {
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                foreach (string key in parameter.Keys)
                    request.AddParameter(key, parameter[key]);
            }

            IRestResponse response = client.Execute(request);
            jsonData = response.Content;

            // 6.检查返回的信息
            // 反序列话
            JsonData data = JsonMapper.ToObject(response.Content);

            // 检查返回的代码
            string code = data["code"].ToString();
            if (code.Equals("200"))
            {
                Debug.Log("GET请求成功！自动尝试次数: " + _tryCount);
                _tryCount = 0;
                return true;
            }
            else
            {
                Debug.LogWarning("GET请求失败！自动尝试次数: " + _tryCount);
                _tryCount++;
                return GET(address, ref jsonData, parameter);
            }
        }


        /// <summary>
        /// POST请求
        /// 请求使用的参数协议: application/json
        /// </summary>
        /// <param name="address">请求地址 例如: "/login"</param>
        /// <param name="jsonData">请求返回的json格式信息</param>
        /// <param name="jsonParameter">请求使用到的json格式参数</param>
        /// <returns>true/false 请求成功/失败</returns>
        public static bool POST(string address, ref string jsonData, string jsonParameter = null)
        {
            return POST(address, ref jsonData, jsonParameter, true);
        }


        /// <summary>
        /// POST请求
        /// 请求使用的参数协议: application/json
        /// </summary>
        /// <param name="address">请求地址 例如: "/login"</param>
        /// <param name="jsonData">请求返回的json格式信息</param>
        /// <param name="jsonParameter">请求使用到的json格式参数</param>
        /// <param name="containToken">请求是否包含密钥token</param>
        /// <returns>true/false 请求成功/失败</returns>
        private static bool POST(string address, ref string jsonData, string jsonParameter = null,
            bool containToken = true)
        {
            // 1.检查服务地址
            if (string.IsNullOrEmpty(Address))
            {
                Debug.LogWarning("POST请求失败！请求服务地址为空<Address is empty>。");
                return false;
            }

            // 2.检查密钥Token
            if (containToken && string.IsNullOrEmpty(LoginToken))
            {
                Debug.LogWarning("POST请求失败！请求密钥为空<Token is empty>。");
                return false;
            }

            // 3.检查请求地址
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogWarning("POST请求失败！请求地址为空<address is empty>。");
                return false;
            }

            // 4.检查已自动尝试的次数
            if (_tryCount >= TryMaxCount)
            {
                Debug.LogWarning("POST请求失败！自动尝试请求次数已达最大值。");
                _tryCount = 0;
                return false;
            }

            // 5.POST请求
            var client = new RestClient(Address + address);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            if (containToken)
                request.AddHeader("token", LoginToken);
            // 添加参数
            if (!string.IsNullOrEmpty(jsonParameter))
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", jsonParameter, ParameterType.RequestBody);
            }

            IRestResponse response = client.Execute(request);
            jsonData = response.Content;

            // 6.检查返回的信息
            // 反序列话
            JsonData data = JsonMapper.ToObject(response.Content);

            // 检查返回的代码
            string code = data["code"].ToString();
            if (code.Equals("200"))
            {
                Debug.Log("POST请求成功！自动尝试次数: " + _tryCount);
                _tryCount = 0;
                return true;
            }
            else
            {
                Debug.LogWarning("POST请求失败！自动尝试次数: " + _tryCount);
                _tryCount++;
                return POST(address, ref jsonData, jsonParameter, containToken);
            }
        }


        public static string FormatJsonStr(string sourJsonStr)
        {
            //格式化json字符串
            object objx = JsonConvert.DeserializeObject(sourJsonStr);
            return FormatJsonString(objx);
        }

        public static string FormatJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}