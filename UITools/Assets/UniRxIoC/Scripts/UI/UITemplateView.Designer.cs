using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	// Generate Id:6c5f5c4b-808a-4d96-aa8b-5ae603423996
	public partial class UITemplateView
	{
		public const string Name = "UITemplateView";
		
		
		private UITemplateViewData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
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
