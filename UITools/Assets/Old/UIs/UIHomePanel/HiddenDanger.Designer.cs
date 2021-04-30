/****************************************************************************
 * 2021.4 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace WWHY
{
	public partial class HiddenDanger
	{
		[SerializeField] public UnityEngine.UI.Button Check;
		[SerializeField] public UnityEngine.UI.Button NoCheck;
		[SerializeField] public UnityEngine.UI.Button Find;
		[SerializeField] public UnityEngine.UI.Button NoAcceptance;
		[SerializeField] public UnityEngine.UI.Button Acceptance;
		[SerializeField] public UnityEngine.UI.Button Handle;
		[SerializeField] public UnityEngine.UI.Button WaitReview;
		[SerializeField] public UnityEngine.UI.Button Complete;
		[SerializeField] public UnityEngine.UI.Button Info;
		[SerializeField] public UnityEngine.UI.Button History;

		public void Clear()
		{
			Check = null;
			NoCheck = null;
			Find = null;
			NoAcceptance = null;
			Acceptance = null;
			Handle = null;
			WaitReview = null;
			Complete = null;
			Info = null;
			History = null;
		}

		public override string ComponentName
		{
			get { return "HiddenDanger";}
		}
	}
}
