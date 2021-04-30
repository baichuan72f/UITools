/****************************************************************************
 * 2021.4 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace UniRxIoC
{
	public partial class SamplePanel
	{
		[SerializeField] public UnityEngine.UI.Button OpenTextsButton;
		[SerializeField] public Texts Texts;
		[SerializeField] public UnityEngine.UI.Button GetDataButton;

		public void Clear()
		{
			OpenTextsButton = null;
			Texts = null;
			GetDataButton = null;
		}

		public override string ComponentName
		{
			get { return "SamplePanel";}
		}
	}
}
