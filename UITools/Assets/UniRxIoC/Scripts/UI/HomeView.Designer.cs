using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	// Generate Id:e9ce1631-f3d8-4df9-a681-450b624beeb1
	public partial class HomeView
	{
		public const string Name = "HomeView";
		
		[SerializeField]
		public LeftPanel LeftPanel;
		[SerializeField]
		public RightPanel RightPanel;
		[SerializeField]
		public UnityEngine.UI.Button Add;
		[SerializeField]
		public UnityEngine.UI.Button Remove;
		[SerializeField]
		public UnityEngine.UI.InputField Result;
		[SerializeField]
		public UnityEngine.UI.InputField EditorItem;
		
		private HomeViewData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			LeftPanel = null;
			RightPanel = null;
			Add = null;
			Remove = null;
			Result = null;
			EditorItem = null;
			
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
