using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    private static Controller _instance;

    public static Controller Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Controller();
            }

            return _instance;
        }
        set { _instance = value; }
    }

    public Dictionary<string, string> dataDic = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}