using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	public class TestComponentData : UIPanelData
	{
	}
	public partial class TestComponent : UIPanel
	{
		protected override void ProcessMsg(int eventId, QMsg msg)
		{
			throw new System.NotImplementedException();
		}
		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as TestComponentData ?? new TestComponentData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
