using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	// Generate Id:caceef51-5494-418d-9bf1-7ad286cf576a
	public partial class UITemplateView
	{
		public const string Name = "UITemplateView";
		
		[SerializeField]
		public UnityEngine.UI.Button ListItem;
		
		private UITemplateViewData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ListItem = null;
			
			mData = null;
		}
		
		public UITemplateViewData Data
		{
			get
			{
				return mData;
			}
		}
		
		UITemplateViewData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UITemplateViewData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
