using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace WWHY
{
	// Generate Id:bbbb0752-e7df-4f3c-8c8c-fbf72158adc7
	public partial class UIHomePanel
	{
		public const string Name = "UIHomePanel";
		
		[SerializeField]
		public HiddenDanger HiddenDanger;
		[SerializeField]
		public InspectionReport InspectionReport;
		[SerializeField]
		public EventReport EventReport;
		[SerializeField]
		public ItemAlarm ItemAlarm;
		
		private UIHomePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			HiddenDanger = null;
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
