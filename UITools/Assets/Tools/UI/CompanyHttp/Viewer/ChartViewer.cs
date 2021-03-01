using System.Collections;
using System.Collections.Generic;
using System.Data;
using CompanyHttp;
using LitJson;
using UnityEngine;
using XCharts;

namespace CompanyHttp.Viewer
{
    public class ChartViewer : MonoBehaviour,IRequestViewer
    {
        public BaseChart Chart;

        public SerieType SerieType;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GetData(string analysisName)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ChartBinder
    {
        public int SerieIndex;
        public string SerieName;
        public string DataWay;
    }
}