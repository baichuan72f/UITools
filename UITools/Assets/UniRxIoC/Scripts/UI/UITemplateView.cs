using System.IO;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UniRxIoC
{
    public class UITemplateViewData : UIPanelData
    {
    }

    public partial class UITemplateView : UIPanel
    {
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
            Addressables.LoadAssetsAsync<GameObject>("TextGroup", result =>
            {
                Debug.Log(result.name);
                ListItem.gameObject.SetActive(false);
                var item = Instantiate<GameObject>(ListItem.gameObject, ListItem.transform.parent);
                item.SetActive(true);
                item.GetComponentInChildren<Text>().text = result.name;
            });
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