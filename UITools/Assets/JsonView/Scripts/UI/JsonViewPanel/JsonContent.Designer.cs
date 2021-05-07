/****************************************************************************
 * 2021.5 DESKTOP-FN2HM7D
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.Serialization;

namespace JsonView
{
	public partial class JsonContent
	{
		[FormerlySerializedAs("JsonViewItem")] [SerializeField] public JsonContentItem jsonContentItem;

		public void Clear()
		{
			jsonContentItem = null;
		}

		public override string ComponentName
		{
			get { return "JsonView";}
		}
	}
}
