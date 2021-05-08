using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	// Generate Id:095c3188-dc97-47d9-91c8-0067c9cb8df8
	public partial class TestComponent
	{
		public const string Name = "TestComponent";
		
		
		private TestComponentData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
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
