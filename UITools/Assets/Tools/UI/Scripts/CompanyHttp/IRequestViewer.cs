using System.Data;
using LitJson;

namespace CompanyHttp
{
    public interface IRequestViewer
    {
        void GetData(string analysisName);
    }
    public enum ViewType
    {
        text,
        textMeshPro,
        Slider,
        BseChart
    }
}