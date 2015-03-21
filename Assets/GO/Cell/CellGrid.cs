﻿using Gem;
using UnityEngine;

namespace HX
{
	public class CellGrid : MonoBehaviour
	{
		private readonly HexGraph<Cell> mCells = new HexGraph<Cell>();

		public bool TryGet(HexCoor _coor, out HexNode<Cell> _out)
		{
			return mCells.TryGet(_coor, out _out);
		}

		public static void Locate(Transform _transform, HexCoor _coor)
		{
			var _pos = (Vector3)(Vector2)_coor;
			var _posOld = _transform.localPosition;
			_pos.z = _posOld.z;
			_transform.localPosition = _pos;
		}

		public HexNode<Cell> Add(Cell _cell, HexCoor _coor)
		{
			var _hexCell = new HexNode<Cell>(_cell);
			if (!mCells.TryAdd(_coor, _hexCell))
				return null;

			_cell.SetGrid(this);
			_cell.transform.SetParent(transform, false);
			Locate(_cell.transform, _coor);

			var _side = -1;
			foreach (var _neighbor in _hexCell.GetAdjacents())
			{
				++_side;
				if (_neighbor == null) continue;
				_cell.SetNeighbor(_neighbor.data, _side);
			}

			return _hexCell;
		}

		public bool IsRemovable(Cell _cell)
		{
			return _cell.grid == this;
		}

		public void Remove(Cell _cell)
		{
			if (!IsRemovable(_cell)) return;
			_cell.DetachGrid();
		}
	}
}