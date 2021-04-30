using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx;
using UniRxIoC.Example;

namespace UniRxIoC
{
    public class HomeViewData : UIPanelData
    {
    }

    public partial class HomeView : UIPanel
    {
        [Inject] public HomeViewModel Model { get; set; }

        // 新增
        [Inject] public IFileLoader fileLoader { get; set; }


        protected override void ProcessMsg(int eventId, QMsg msg)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as HomeViewData ?? new HomeViewData();
            // please add init code here
            JsonParser.Container.Inject(this);

            // Model -> View
            Model.leftData.Subscribe(json => ShowData(LeftPanel.JsonNodeLeft.gameObject, json))
                .AddTo(this);
            Model.rightData.Subscribe(json => ShowData(RightPanel.JsonNodeRight.gameObject, json))
                .AddTo(this);

            // View -> Model
            // 这里使用 UniRx 风格
            LeftPanel.ReadFileBtn.OnClickAsObservable()
                .Subscribe((_) => fileLoader.RequestSomeData(LeftPanel.FilePathInput.text,
                    d =>
                    {
                        d.LogInfo();
                        if (!string.IsNullOrEmpty(d))
                        {
                            JObject.Parse(d.Trim());
                            Model.leftData.Value = JObject.Parse(d.Trim());
                        }
                    }));

            // 新增
            LeftPanel.ShowJsonBtn.OnClickAsObservable()
                .Subscribe((_) => { Debug.Log(Model.leftData); });
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

        public void ShowData(GameObject item, JObject json)
        {
            if (json == null)
            {
                return;
            }

            for (int i = 0; i < item.transform.parent.childCount; i++)
            {
                item.transform.parent.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < json.Count; i++)
            {
                if (string.IsNullOrEmpty(json[i].ToString()))
                {
                    continue;
                }

                GameObject itemObj = null;
                if (i < item.transform.parent.childCount)
                {
                    itemObj = item.transform.parent.GetChild(i).gameObject;
                }
                else
                {
                    itemObj = GameObject.Instantiate<GameObject>(item);
                }

                itemObj.GetComponentInChildren<Text>().text = json[i].ToString();
                itemObj.SetActive(true);
            }
        }
    }
}