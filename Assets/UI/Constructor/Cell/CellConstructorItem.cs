using Gem;
using UnityEngine;

namespace ui
{
	public class CellConstructorItem : IConstructorItem
	{
		private readonly CellPartData m_Data;
		public CellGrid cellGrid;
		public Cell cellPrf;

		public CellConstructorItem(CellPartData _data)
		{
			m_Data = _data;
		}

		public string name { get { return m_Data.name_;  } }

		public GameObject MakeItem()
		{
			if (m_Data.constructorItemPrf)
			{
				return (GameObject)Object.Instantiate(m_Data.constructorItemPrf);
			}
			else
			{
				var _item = new GameObject(name);
				var _renderer = _item.AddComponent<UI2DSprite>();
				_renderer.sprite2D = m_Data.sprite;
				_renderer.MakePixelPerfect();
				_renderer.transform.localScale = new Vector3(0.4f, 0.4f, 1);
				return _item;
			}
		}

		public DragAndDrop MakeDragAndDrop()
		{
			var _cellPart = m_Data.MakeGO();
			var _dnd = CellHelper.AddDragAndDrop(_cellPart);
			_dnd.cellPrf = cellPrf;
			_dnd.cellGrid = cellGrid;
			_dnd.destroyIfFailed = true;
			return _dnd;
		}
	}
}