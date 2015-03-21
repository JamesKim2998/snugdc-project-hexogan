using System;
using Gem;
using UnityEngine;

namespace HX.UI
{
	public class ConstructorEntityView : MonoBehaviour
	{
		public GameObject itemPivot;
		public UILabel nameLabel;

		public Action onPressDown;

		public void Setup(IConstructorItem _data)
		{
			nameLabel.text = _data.name;
			var _item = _data.MakeItem();
			_item.transform.SetParentIdentity(itemPivot.transform);
		}

		public void ListenPressDown()
		{
			if (onPressDown != null) onPressDown();
		}
	}
}