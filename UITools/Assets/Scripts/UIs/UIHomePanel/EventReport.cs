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

        protected override void OnShow()
        {
            base.OnShow();
            History.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "History", TimeSpan.FromDays(1));
            });
            Check.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "Check", TimeSpan.FromDays(1));
            });
            NoCheck.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "NoCheck", TimeSpan.FromDays(1));
            });
            Find.onClick.AddListener(() => { HomePanelView.OpenHistory("EventReport", "Find", TimeSpan.FromDays(1)); });
            NoAcceptance.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "NoAcceptance", TimeSpan.FromDays(1));
            });
            Acceptance.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "Acceptance", TimeSpan.FromDays(1));
            });
            Handle.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "Handle", TimeSpan.FromDays(1));
            });
            WaitReview.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "WaitReview", TimeSpan.FromDays(1));
            });
            Complete.onClick.AddListener(() =>
            {
                HomePanelView.OpenHistory("EventReport", "Complete", TimeSpan.FromDays(1));
            });
        }

        protected override void OnBeforeDestroy()
        {
        }
    }
}