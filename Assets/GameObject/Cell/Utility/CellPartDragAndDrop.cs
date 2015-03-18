using Gem;
using UnityEngine;

namespace HX
{
	public abstract class CellPartDragAndDrop : DragAndDrop
	{
		public CellGrid cellGrid;
		public Cell cellPrf;

		public bool destroyIfFailed = false;

		private bool? m_ColliderOrgTriggerFlag;

		protected override void _OnMouseDown()
		{
			base._OnMouseDown();
			transform.position += Vector3.back;
			m_ColliderOrgTriggerFlag = GetComponent<Collider2D>().isTrigger;
			GetComponent<Collider2D>().isTrigger = true;
		}

		protected override void _OnMouseDrag()
		{
			base._OnMouseDrag();
			Pivot();
		}

		protected override void _OnMouseUp()
		{
			base._OnMouseUp();
			if (Pivot(true))
			{
				transform.position += Vector3.forward;
				if (m_ColliderOrgTriggerFlag.HasValue)
					GetComponent<Collider2D>().isTrigger = m_ColliderOrgTriggerFlag.Value;
			}
			else
			{
				if (destroyIfFailed)
					Destroy(gameObject);
			}
		}

		protected virtual bool IsLocatable(HexCell<Cell> _cell, HexCoor _coor)
		{
			return true;
		}

		protected void Locate(HexCoor _coor)
		{
			var _posNew = cellGrid.transform.localToWorldMatrix.MultiplyPoint((Vector2)_coor);
			var _posOld = transform.position;
			_posNew.z = _posOld.z;
			transform.position = _posNew;

			var _angleNew = transform.eulerAngles.z;
			var _angleOld = transform.eulerAngles;
			_angleOld.z = _angleNew;
			transform.eulerAngles = _angleOld;
		}

		protected abstract bool Attach(HexCell<Cell> _cell, HexCoor _coor);

		bool Pivot(bool _attach = false)
		{
			var _screenPos = Input.mousePosition + (Vector3)offset;
			var _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);

			var _hexPos = cellGrid.transform.worldToLocalMatrix.MultiplyPoint(transform.position);
			var _hexCoor = HexCoor.Round(_hexPos);

			HexCell<Cell> _cell;
			cellGrid.TryGet(_hexCoor, out _cell);

			if (!IsLocatable(_cell, _hexCoor))
				return false;

			if (_attach)
			{
				if (Attach(_cell, _hexCoor))
				{
					Destroy(this);
					return true;
				}
			}

			Locate(_hexCoor);

			return true;
		}
	}
}