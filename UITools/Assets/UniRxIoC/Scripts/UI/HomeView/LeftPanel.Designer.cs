/****************************************************************************
 * 2021.4 DESKTOP-FN2HM7D
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
		[SerializeField] public UnityEngine.UI.ScrollRect JsonView;
		[SerializeField] public JsonNodeLeft JsonNodeLeft;

		public void Clear()
		{
			FilePathInput = null;
			ReadFileBtn = null;
			ShowJsonBtn = null;
			JsonView = null;
			JsonNodeLeft = null;
		}

		public override string ComponentName
		{
			get { return "LeftPanel";}
		}
	}
}
