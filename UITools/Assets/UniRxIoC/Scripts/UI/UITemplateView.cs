using System.IO;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
    public class UITemplateViewData : UIPanelData
    {
    }

    public partial class UITemplateView : UIPanel
    {
        public string path = "/Commponent/";

        protected override void ProcessMsg(int eventId, QMsg msg)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UITemplateViewData ?? new UITemplateViewData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            //var assets = 
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}