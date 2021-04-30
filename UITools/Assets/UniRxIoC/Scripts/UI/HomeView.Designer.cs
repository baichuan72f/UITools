using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	// Generate Id:bed76cd0-0b80-424a-a51a-e62f691e3543
	public partial class HomeView
	{
		public const string Name = "HomeView";
		
		[SerializeField]
		public LeftPanel LeftPanel;
		[SerializeField]
		public RightPanel RightPanel;
		
		private HomeViewData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			LeftPanel = null;
			RightPanel = null;
			
			mData = null;
		}
		
		public HomeViewData Data
		{
			get
			{
				return mData;
			}
		}
		
		HomeViewData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new HomeViewData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
