using System;
using UnityEngine;
using System.Collections;

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
			TransformHelper.SetParentIdentity(_item, itemPivot);
		}

		public void ListenPressDown()
		{
			if (postPressDown != null) postPressDown();
		}
	}
}