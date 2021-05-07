/****************************************************************************
 * 2021.5 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	public partial class RightPanel
	{
		[SerializeField] public UnityEngine.UI.InputField FilePathInput;
		[SerializeField] public UnityEngine.UI.Button LoadData;
		[SerializeField] public UnityEngine.UI.Button Bind;
		[SerializeField] public JsonViewRight JsonViewRight;

		public void Clear()
		{
			FilePathInput = null;
			LoadData = null;
			Bind = null;
			JsonViewRight = null;
		}

		public override string ComponentName
		{
			get { return "RightPanel";}
		}
	}
}
