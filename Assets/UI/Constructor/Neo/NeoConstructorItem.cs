using ui;
using UnityEngine;
using System.Collections;

namespace ui
{
	public class NeoConstructorItem : IConstructorItem
	{
		private readonly NeoMechanicData m_Data;

		public NeoConstructorItem(NeoMechanicData _data)
		{
			m_Data = _data;
		}

		public string name { get { return m_Data.name_; } }

		public GameObject MakeItem()
		{
			if (m_Data.constructorItemPrf)
			{
				return (GameObject) Object.Instantiate(m_Data.constructorItemPrf);
			}
			else
			{
				var _item = new GameObject(name);
				var _renderer = _item.AddComponent<UI2DSprite>();
				_renderer.sprite2D = m_Data.sprite;
				_renderer.MakePixelPerfect();
				return _item;
			}
		}

		public DragAndDrop MakeDragAndDrop()
		{
			var _mechanic = m_Data.MakeMechanic();
			var _dnd = NeoMechanicHelper.AddDragAndDrop(_mechanic);
			_dnd.destroyIfFailed = true;
			return _dnd;
		}
	}

}