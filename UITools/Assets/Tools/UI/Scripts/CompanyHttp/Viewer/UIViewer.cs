using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace CompanyHttp.Viewer
{
    public class UIViewer : MonoBehaviour,IRequestViewer
    {
        public Text Text;
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
            Text.text = analysisName;
        }
    }
}
