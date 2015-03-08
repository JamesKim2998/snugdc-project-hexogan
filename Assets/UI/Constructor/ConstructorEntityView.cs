using System;
using Gem;
using UnityEngine;

namespace ui
{
	public class ConstructorEntityView : MonoBehaviour
	{
		public GameObject itemPivot;
		public UILabel nameLabel;

		public Action postPressDown;

		public void Setup(IConstructorItem _data)
		{
			nameLabel.text = _data.name;
			var _item = _data.MakeItem();
			_item.transform.SetParentIdentity(itemPivot.transform);
		}

		public void ListenPressDown()
		{
			if (postPressDown != null) postPressDown();
		}
	}
}