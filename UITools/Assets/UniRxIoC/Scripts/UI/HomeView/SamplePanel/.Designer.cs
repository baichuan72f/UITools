/****************************************************************************
 * 2021.4 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace WWHY
{
	public partial class 
	{
		[SerializeField] public UnityEngine.UI.Text Text1;
		[SerializeField] public UnityEngine.UI.Text Text2;
		[SerializeField] public UnityEngine.UI.Text Text3;

		public void Clear()
		{
			Text1 = null;
			Text2 = null;
			Text3 = null;
		}

		public override string ComponentName
		{
			get { return "";}
		}
	}
}
