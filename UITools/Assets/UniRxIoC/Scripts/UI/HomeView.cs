using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonView;
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
            Model.leftData.Subscribe(json => { })
                .DisposeWhenGameObjectDestroyed(this);
            Model.rightData.Subscribe(json => { })
                .DisposeWhenGameObjectDestroyed(this);
            Model.editItem.Subscribe(json => { EditorItem.text = json.ToJson(); }).DisposeWhenGameObjectDestroyed(this);

            // View -> Model
            // 这里使用 UniRx 风格
            LeftPanel.ReadFileBtn.OnClickAsObservable()
                .Subscribe((_) => fileLoader.RequestSomeData(LeftPanel.FilePathInput.text,
                    d =>
                    {
                        d.LogInfo();
                        if (!string.IsNullOrEmpty(d))
                        {
                            var data = JObject.Parse(d.Trim());
                            ShowData(LeftPanel.JsonViewLeft.gameObject, data);
                        }
                    }));
            RightPanel.LoadData.OnClickAsObservable()
                .Subscribe((_) => fileLoader.RequestSomeData(RightPanel.FilePathInput.text,
                    d =>
                    {
                        d.LogInfo();
                        if (!string.IsNullOrEmpty(d))
                        {
                            var data = JObject.Parse(d.Trim());
                            ShowData(RightPanel.JsonViewRight.gameObject, data);
                        }
                    }));
            Add.OnClickAsObservable().Subscribe(_ =>
            {
                var keyStr = LeftPanel.JsonViewLeft.GetComponentInChildren<JsonContent>().selectStr;
                var valueStr = RightPanel.JsonViewRight.GetComponentInChildren<JsonContent>().selectStr;
                if (!string.IsNullOrEmpty(keyStr))
                {
                    if (Model.editItem.Value.ContainsKey(keyStr))
                    {
                        var v = Model.editItem.Value;
                        v[keyStr] = valueStr;
                        Model.editItem.Value = new JObject(v);
                    }
                    else
                    {
                        var v = Model.editItem.Value;
                        v.Add(keyStr, valueStr);
                        Model.editItem.Value = new JObject(v);
                    }
                }
            });
            Remove.OnClickAsObservable().Subscribe(_ =>
            {
                var keyStr = LeftPanel.JsonViewLeft.GetComponentInChildren<JsonContent>().selectStr;
                if (!string.IsNullOrEmpty(keyStr) && Model.editItem.Value.ContainsKey(keyStr))
                {
                    var v = Model.editItem.Value;
                    v.Remove(keyStr);
                    Model.editItem.Value = new JObject(v);
                }
            });
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

            item.GetComponentInChildren<JsonContent>().ShowData(0, json);
        }
    }
}