using Gem;
using UnityEngine;

namespace HX.UI
{
	public class NeoConstructorItem : IConstructorItem
	{
		private readonly NeoMechanicData mData;

		public NeoConstructorItem(NeoMechanicData _data)
		{
			mData = _data;
		}

		public string name { get { return mData.name; } }

		public GameObject MakeItem()
		{
			if (mData.constructorItemPrf)
			{
				return (GameObject) Object.Instantiate(mData.constructorItemPrf);
			}
			else
			{
				var _item = new GameObject(name);
				var _renderer = _item.AddComponent<UI2DSprite>();
				_renderer.sprite2D = mData.sprite;
				_renderer.MakePixelPerfect();
				return _item;
			}
		}

		public DragAndDrop MakeDragAndDrop()
		{
			var _mechanic = mData.MakeMechanic();
			var _dnd = NeoMechanicHelper.AddDragAndDrop(_mechanic);
			_dnd.destroyIfFailed = true;
			return _dnd;
		}
	}

}