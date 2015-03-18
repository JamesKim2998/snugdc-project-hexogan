using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;

namespace Gem
{
	public class HexGraph<T> : IEnumerable<KeyValuePair<HexCoor, HexNode<T>>>
	{
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
			public HexIdx side;
			public HexNode<T> node;
		}

		public IEnumerable<Neighbor> Neighbors(HexCoor _coor)
		{
			var _side = 0;
			foreach (var _neighbor in _coor.Neighbors())
			{
				HexNode<T> _node;
				if (TryGet(_neighbor, out _node))
					yield return new Neighbor { node = _node, side = (HexIdx)_side };
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
			if (_node == null) return;
			Remove(_node);
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
	}
}