/****************************************************************************
 * 2021.4 DESKTOP-FN2HM7D
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace WWHY
{
    public partial class EventReport : UIElement
    {
        private void Awake()
        {
        }

        public void BindClick()
        {
            History.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", -2, 30); });
            Info.onClick.AddListener(() => { HomePanelView.GetStreamInfo("EventReport", 196); });
            Check.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 0, 1); });
            NoCheck.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 1, 1); });
            Find.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 1, 1); });
            NoAcceptance.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 2, 1); });
            Acceptance.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 3, 1); });
            Handle.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 3, 1); });
            WaitReview.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 4, 1); });
            Complete.onClick.AddListener(() => { HomePanelView.GetStreamInfoList("EventReport", 4, 1); });
        }

        protected override void OnBeforeDestroy()
        {
        }
    }
}