using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using UnityEngine;

namespace Gem
{
	public class HexGraph<T> : IEnumerable<KeyValuePair<HexCoor, HexNode<T>>>
	{
		public bool empty { get { return mNodes.Empty(); } }
		public int count { get { return mNodes.Count; } }

		public bool allowIsland = false;
		private readonly BiDictionary<HexCoor, HexNode<T>> mNodes = new BiDictionary<HexCoor, HexNode<T>>();

		public HexNode<T> this[HexCoor _coor]
		{
			get { return Get(_coor); }
		}

		public IEnumerator<KeyValuePair<HexCoor, HexNode<T>>> GetEnumerator()
		{
			return mNodes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return mNodes.GetEnumerator();
		}

		public bool TryGet(HexCoor _coor, out HexNode<T> _out)
		{
			return mNodes.TryGetValue(_coor, out _out);
		}

		public HexNode<T> Get(HexCoor _coor)
		{
			HexNode<T> _out;
			if (TryGet(_coor, out _out))
			{
				return _out;
			}
			else
			{
				L.E("node is not exists for coor " + _coor + ". return null.");
				return null;
			}
		}

		public bool Contains(HexNode<T> _node)
		{
			return mNodes.Reverse.ContainsKey(_node);
		}

		public struct Neighbor
		{
			public HexEdge side;
			public HexNode<T> node;
		}

		public IEnumerable<Neighbor> Neighbors(HexCoor _coor)
		{
			var _side = 0;
			foreach (var _neighbor in _coor.Neighbors())
			{
				HexNode<T> _node;
				if (TryGet(_neighbor, out _node))
					yield return new Neighbor { node = _node, side = (HexEdge)_side };
				++_side;
			}
		}

		public bool CheckAddable(HexCoor _coor, HexNode<T> _node)
		{
			if (mNodes.Count == 0)
				return true;

			HexNode<T> _temp;
			if (TryGet(_coor, out _temp))
			{
				L.E("there is already node in " + _coor + ". ignore.");
				return false;
			}

			if (Contains(_node))
			{
				L.E("trying to add node. But already exists. iIgnore.");
				return false;
			}

			if (allowIsland && !Neighbors(_coor).Any())
			{
				L.E("there is no neighbor around " + _coor + ". ignore.");
				return false;
			}

			return true;
		}

		public bool TryAdd(HexCoor _coor, HexNode<T> _node)
		{
			if (!CheckAddable(_coor, _node))
				return false;
			Add(_coor, _node);
			return true;
		}

		public void Add(HexCoor _coor, HexNode<T> _node)
		{
			mNodes.Add(_coor, _node);
			foreach (var _neighbor in Neighbors(_coor))
				_node.Connect(_neighbor.node, _neighbor.side);
		}

		public void Remove(HexCoor _coor)
		{
			var _node = Get(_coor);

			if (_node == null)
			{
				L.W("node for " + _coor + " not found.");
				return;
			}

			Remove(_node);
		}

		public T GetAndRemove(HexCoor _coor)
		{
			var _node = Get(_coor);

			if (_node == null)
			{
				L.W("node for " + _coor + " not found.");
				return default(T);
			}

			Remove(_node);
			return _node.data;
		}

		public void Remove(HexNode<T> _node)
		{
			mNodes.Reverse.Remove(_node);
			_node.DisconnectAll();
		}

		public HashSet<HexNode<T>> RemoveIslands()
		{
			var _checked = new HashSet<HexNode<T>>();
			var _traversing = new HashSet<HexNode<T>> { Get(HexCoor.ZERO) };

			while (_traversing.Count != 0)
			{
				var _node = _traversing.First();

				if (!_checked.Contains(_node))
				{
					foreach (var _neighbor in _node.GetNeighbors())
						_traversing.Add(_neighbor);
					_checked.Add(_node);
				}

				_traversing.Remove(_node);
			}

			var _island = new HashSet<HexNode<T>>();
			foreach (var _cell in this)
			{
				if (!_checked.Contains(_cell.Value))
					_island.Add(_cell.Value);
			}

			foreach (var _cell in _island)
				Remove(_cell);

			return _island;
		}

		public void Clear()
		{
			foreach (var _cell in mNodes)
				_cell.Value.DisconnectAll();
			mNodes.Clear();
		}

		public IEnumerable<KeyValuePair<HexCoor, HexNode<T>>> Overlaps(Rect _rect, bool _includeEmpty)
		{
			var _bl = HexCoor.Round(_rect.min);
			var _tr = HexCoor.Round(_rect.max);

			var _blOffset = _bl - _rect.min;
			var _trOffset = _tr - _rect.max;

			HexQ _firstQ;
			HexQ _lastQ;

			if (_blOffset.y <= 0.5f * HexHelper.TAN_30)
				_firstQ = _bl.q;
			else
				_firstQ = _bl.q - 1;

			if (_trOffset.y >= -0.5f * HexHelper.TAN_30)
				_lastQ = _tr.q;
			else
				_lastQ = _tr.q + 1;

			for (var q = _firstQ; q <= _lastQ; ++q)
			{
				int _bottomToLine = (int)q - (int)_bl.q;
				int _topToLine = (int)_tr.q - (int)q;

				HexP _firstP;
				HexP _lastP;

				if (_bottomToLine%2 == 0)
				{
					_firstP = _bl.p - _bottomToLine/2;
				}
				else
				{
					_firstP = _bl.p - (_bottomToLine - 1)/2;
					if (_blOffset.x > 0)
						_firstP -= 1;
				}

				if (_topToLine%2 == 0)
				{
					_lastP = _tr.p + _topToLine/2;
				}
				else
				{
					_lastP = _tr.p + (_topToLine - 1)/2;
					if (_trOffset.x < 0)
						_lastP += 1;
				}

				for (var p = _firstP; p <= _lastP; ++p)
				{
					var _coor = new HexCoor(p, q);
					HexNode<T> _node;
					TryGet(_coor, out _node);

					if (!_includeEmpty && (_node == null))
						continue;

					yield return new KeyValuePair<HexCoor, HexNode<T>>(_coor, _node);
				}
			}
		}
	}
}