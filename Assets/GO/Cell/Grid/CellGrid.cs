﻿using System.Collections;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{
	public class CellGrid : MonoBehaviour, IEnumerable<KeyValuePair<HexCoor, Cell>>
	{
		public bool empty { get { return mCells.empty; } }
		public int count { get { return mCells.count; } }

		private readonly HexGraph<Cell> mCells = new HexGraph<Cell>();

		void Awake()
		{
			mCells.allowIsland = true;
		}

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
			var _node = new HexNode<Cell>(_cell);
			if (!mCells.TryAdd(_coor, _node))
				return null;

			_cell.SetGrid(this, _node);
			_cell.transform.SetParent(transform, false);
			Locate(_cell.transform, _coor);

			return _node;
		}

		public bool IsDetachable(Cell _cell)
		{
			return _cell.grid == this;
		}

		public void Detach(Cell _cell)
		{
			if (!IsDetachable(_cell)) return;
			_cell.DetachGrid();
		}

		public void DestroyAll()
		{
			foreach (var kv in mCells)
				Destroy(kv.Value.data.gameObject);
			mCells.Clear();
		}

		public IEnumerable<KeyValuePair<HexCoor, Cell>> Overlaps(Rect _rect, bool _includeEmpty)
		{
			foreach (var _kv in mCells.Overlaps(_rect, _includeEmpty))
			{
				var _node = _kv.Value;

				if (_node != null)
					yield return new KeyValuePair<HexCoor, Cell>(_kv.Key, _kv.Value.data);
				else if (_includeEmpty)
					yield return new KeyValuePair<HexCoor, Cell>(_kv.Key, null);
			}
		}

		public IEnumerator<KeyValuePair<HexCoor, Cell>> GetEnumerator()
		{
			foreach (var _kv in mCells)
				yield return new KeyValuePair<HexCoor, Cell>(_kv.Key, _kv.Value.data);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}