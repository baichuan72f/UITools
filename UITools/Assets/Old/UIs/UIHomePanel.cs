using NetWork;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace WWHY
{
    public class UIHomePanelData : UIPanelData
    {
    }

    public partial class UIHomePanel : UIPanel
    {
        protected override void ProcessMsg(int eventId, QMsg msg)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHomePanelData ?? new UIHomePanelData();
            // please add init code here

            HiddenDanger.BindClick();
            InspectionReport.BindClick();
            EventReport.BindClick();
            ItemAlarm.BindClick();
            RestSharpProxy.Instance.init("16833336666", "111111");
        }

        protected override void OnOpen(IUIData uiData = null)
        {
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