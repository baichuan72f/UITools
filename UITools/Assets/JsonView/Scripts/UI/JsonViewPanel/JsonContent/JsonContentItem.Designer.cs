/****************************************************************************
 * 2021.5 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace JsonView
{
	public partial class JsonContentItem
	{
		[SerializeField] public UnityEngine.UI.Image ItemContent;
		[SerializeField] public UnityEngine.UI.Toggle Toggle;
		[SerializeField] public UnityEngine.UI.Image Space;
		[SerializeField] public UnityEngine.UI.Text Key;
		[SerializeField] public UnityEngine.UI.Text Perporty;
		[SerializeField] public UnityEngine.UI.Text Value;
		[SerializeField] public UnityEngine.UI.Text ArrayStart;
		[SerializeField] public UnityEngine.UI.Text ArrayEnd;
		[SerializeField] public UnityEngine.UI.Text ObjectStart;
		[SerializeField] public UnityEngine.UI.Text ObjectEnd;
		[SerializeField] public UnityEngine.UI.Text Next;
		[SerializeField] public UnityEngine.UI.Text More;

		public void Clear()
		{
			ItemContent = null;
			Toggle = null;
			Space = null;
			Key = null;
			Perporty = null;
			Value = null;
			ArrayStart = null;
			ArrayEnd = null;
			ObjectStart = null;
			ObjectEnd = null;
			Next = null;
			More = null;
		}

		public override string ComponentName
		{
			get { return "JsonViewItem";}
		}
	}
}
