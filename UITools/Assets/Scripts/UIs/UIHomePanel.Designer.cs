using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace WWHY
{
	// Generate Id:bf77bc1b-37a9-4228-9c1e-565cec3d05b0
	public partial class UIHomePanel
	{
		public const string Name = "UIHomePanel";
		
		[SerializeField]
		public HiddenDanger ModuleHiddenDanger;
		[SerializeField]
		public InspectionReport InspectionReport;
		[SerializeField]
		public EventReport EventReport;
		[SerializeField]
		public ItemAlarm ItemAlarm;
		
		private UIHomePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ModuleHiddenDanger = null;
			InspectionReport = null;
			EventReport = null;
			ItemAlarm = null;
			
			mData = null;
		}
		
		public UIHomePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHomePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHomePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
