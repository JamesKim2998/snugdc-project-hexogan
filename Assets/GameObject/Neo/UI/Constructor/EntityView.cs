using System;
using UnityEngine;
using System.Collections;

namespace neo.ui.constructor
{
	public class EntityView : MonoBehaviour
	{
		public GameObject itemPivot;
		public UILabel nameLabel;

		public Action postPressDown;

		public void Setup(NeoMechanicData _data)
		{
			nameLabel.text = _data.name_;
			var _item = _data.MakeConstructorItem();
			TransformHelper.SetParentIdentity(_item, itemPivot);
		}

		public void ListenPressDown()
		{
			if (postPressDown != null) postPressDown();
		}
	}
}