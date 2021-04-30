/****************************************************************************
 * 2021.4 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	public partial class RightPanel
	{
		[SerializeField] public UnityEngine.UI.InputField FilePathInput;
		[SerializeField] public UnityEngine.UI.Button UpdateKeyBtn;
		[SerializeField] public UnityEngine.UI.Button SaveConfigBtn;
		[SerializeField] public UnityEngine.UI.ScrollRect JsonView;
		[SerializeField] public JsonNodeRight JsonNodeRight;

		public void Clear()
		{
			FilePathInput = null;
			UpdateKeyBtn = null;
			SaveConfigBtn = null;
			JsonView = null;
			JsonNodeRight = null;
		}

		public override string ComponentName
		{
			get { return "RightPanel";}
		}
	}
}
