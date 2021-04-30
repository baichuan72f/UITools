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
	public partial class InspectionReport : UIElement
	{
		private void Awake()
		{
		}
		public void BindClick()
		{
			History.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, -2, 30); });
			Info.onClick.AddListener(() => { HomePanelView.GetStreamInfo(ComponentName, 218); });
			Check.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 0, 1); });
			NoCheck.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 1, 1); });
			Find.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 1, 1); });
			NoAcceptance.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 2, 1); });
			Acceptance.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 3, 1); });
			Handle.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 3, 1); });
			WaitReview.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 4, 1); });
			Complete.onClick.AddListener(() => { HomePanelView.GetStreamInfoList(ComponentName, 4, 1); });
		}
		protected override void OnBeforeDestroy()
		{
		}
	}
}