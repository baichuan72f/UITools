using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	// Generate Id:f62eaa31-c949-4fe3-b843-691ac2cff8d2
	public partial class TestComponent
	{
		public const string Name = "TestComponent";
		
		[SerializeField]
		public UnityEngine.UI.Text Text0;
		[SerializeField]
		public UnityEngine.UI.Text Text1;
		[SerializeField]
		public UnityEngine.UI.Text Text2;
		[SerializeField]
		public UnityEngine.UI.Text Text3;
		[SerializeField]
		public UnityEngine.UI.Text Text4;
		
		private TestComponentData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Text0 = null;
			Text1 = null;
			Text2 = null;
			Text3 = null;
			Text4 = null;
			
			mData = null;
		}
		
		public TestComponentData Data
		{
			get
			{
				return mData;
			}
		}
		
		TestComponentData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new TestComponentData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
