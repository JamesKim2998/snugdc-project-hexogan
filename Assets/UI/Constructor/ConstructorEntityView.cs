using System;
using UnityEngine;

namespace HX.UI
{
	public class ConstructorEntityView : MonoBehaviour
	{
		public Vector2 itemPosition;
		public UILabel nameLabel;

		public Action onPressDown;

		public void Setup(IConstructorItem _data)
		{
			nameLabel.text = _data.name;
			var _item = _data.MakeItem();
			_item.transform.SetParent(transform, false);
			_item.transform.localPosition = itemPosition;
		}

		public void ListenPressDown()
		{
			if (onPressDown != null) onPressDown();
		}
	}
}