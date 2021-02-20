using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DataReader : MonoBehaviour
{
    public InputField UrlField;
    public InputField PathField;
    public Dropdown MethodField;
    public Dropdown ContetnTypeField;
    public InputField requestBodyField;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            excute();
        }
    }

    public void excute()
    {
        Debug.Log(requestBodyField.text);
        var data = JObject.Parse(requestBodyField.text);
        simpleRequest(data);
    }
    public string simpleRequest(JObject requestData)
    {
        var address = UrlField.text;
        var url = PathField.text;

        var client = new RestClient(address.ToString() + url.ToString());
        client.Timeout = -1;
        var method = (Method) Enum.Parse(typeof(Method), MethodField.options[MethodField.value].text);
        var request = new RestRequest(method);
        request.AddHeader("Content-Type", ContetnTypeField.options[ContetnTypeField.value].text);

        var body = requestData["Body"] as JObject;
        request.AddParameter("application/json", requestData.ToString(), ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        Debug.Log(response.Content);
        return response.Content;
    }

    public object getValue(string data, string type)
    {
        switch (type)
        {
            case "Byte": return Convert.ToByte(data);
            case "Boolean": return Convert.ToBoolean(data);
            case "Int": return Convert.ToInt32(data);
            case "float": return Convert.ToSingle(data);
            case "Double": return Convert.ToDouble(data);
            case "DateTime": return Convert.ToDateTime(data);
            case "Char": return Convert.ToChar(data);
            case "String": return Convert.ToString(data);
        }

        return null;
    }
}