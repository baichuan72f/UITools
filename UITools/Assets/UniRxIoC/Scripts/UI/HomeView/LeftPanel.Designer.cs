/****************************************************************************
 * 2021.5 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	public partial class LeftPanel
	{
		[SerializeField] public UnityEngine.UI.InputField FilePathInput;
		[SerializeField] public UnityEngine.UI.Button ReadFileBtn;
		[SerializeField] public UnityEngine.UI.Button ShowJsonBtn;
		[SerializeField] public JsonViewLeft JsonViewLeft;

		public void Clear()
		{
			FilePathInput = null;
			ReadFileBtn = null;
			ShowJsonBtn = null;
			JsonViewLeft = null;
		}

		public override string ComponentName
		{
			get { return "LeftPanel";}
		}
	}
}
