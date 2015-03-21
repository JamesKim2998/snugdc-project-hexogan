using Gem;
using UnityEngine;

namespace HX.UI
{
	public class CellConstructorItem : IConstructorItem
	{
		private readonly CellPartData mData;
		public CellGrid cellGrid;
		public Cell cellPrf;

		public CellConstructorItem(CellPartData _data)
		{
			mData = _data;
		}

		public string name { get { return mData.name;  } }

		public GameObject MakeItem()
		{
			if (mData.constructorItemPrf)
			{
				return Object.Instantiate(mData.constructorItemPrf);
			}
			else
			{
				var _item = new GameObject(name);
				var _renderer = _item.AddComponent<UI2DSprite>();
				_renderer.sprite2D = mData.sprite;
				_renderer.MakePixelPerfect();
				_renderer.transform.localScale = new Vector3(0.4f, 0.4f, 1);
				return _item;
			}
		}

		public DragAndDrop MakeDragAndDrop()
		{
			var _cellPart = mData.MakeGO();
			var _dnd = CellHelper.AddDragAndDrop(_cellPart);
			_dnd.cellPrf = cellPrf;
			_dnd.cellGrid = cellGrid;
			_dnd.destroyIfFailed = true;
			return _dnd;
		}
	}
}