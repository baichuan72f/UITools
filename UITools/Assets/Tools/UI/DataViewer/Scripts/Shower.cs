using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shower : MonoBehaviour
{
    public string url;
    public string path;

    public int time;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (time == 2 && Time.frameCount % 100 == 0)
        {
            if (Controller.Instance.dataDic.ContainsKey(url))
            {
                GetComponent<Text>().text = Controller.Instance.dataDic[url];
            }
        }
    }
}
