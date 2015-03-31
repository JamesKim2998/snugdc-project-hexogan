using System.Collections.Generic;

namespace Gem
{
	public class HexNode<T>
	{
		public T data { get; private set; }
		private readonly HexNode<T>[] mNeighbors = new HexNode<T>[6];

		public HexNode(T _data)
		{
			data = _data;
		}

		~HexNode()
		{
			DisconnectAll();
		}

		public bool IsBridge()
		{
			var _cnt = 0;
			var _first = false;
			bool? _old = null;

			foreach (var _neighbor in GetNeighbors())
			{
				var _new = _neighbor != null;

				if (_old.HasValue)
				{
					if (!_new && _old.Value)
						if (++_cnt > 1) return true;
				}
				else
				{
					_first = _new;
				}

				_old = _new;
			}

			if (_old.HasValue && !_first && _old.Value)
				if (++_cnt > 1) return true;

			return false;
		}

		public HexNode<T> GetNeighbor(HexEdge _idx)
		{
			return mNeighbors[(int)_idx];
		}

		public IEnumerable<HexNode<T>> GetAdjacents()
		{
			for (var _i = 0; _i != 6; ++_i)
				yield return mNeighbors[_i];
		}

		public IEnumerable<HexNode<T>> GetNeighbors()
		{
			for (var _i = 0; _i != 6; ++_i)
			{
				var _neighbor = mNeighbors[_i];
				if (_neighbor != null)
					yield return _neighbor;
			}
		}

		public void Connect(HexNode<T> _node, HexEdge _idx)
		{
			if (GetNeighbor(_idx) != null)
			{
				L.E("there is already mechanic exists in " + _idx + ". ignore.");
				return;
			}

			mNeighbors[(int)_idx] = _node;
			_node.mNeighbors[(int)_idx.Opposite()] = this;
		}

		public void Disconnect(HexEdge _idx)
		{
			if (GetNeighbor(_idx) == null)
			{
				L.W("there is no neighbor in " + _idx + ". ignore.");
				return;
			}

			var _neighbor = GetNeighbor(_idx);
			_neighbor.mNeighbors[(int)_idx.Opposite()] = null;
			mNeighbors[(int)_idx] = null;
		}

		public void DisconnectAll()
		{
			foreach (var i in HexHelper.GetEdges())
			{
				var _neighbor = GetNeighbor(i);
				if (_neighbor != null)
				{
					_neighbor.mNeighbors[(int)i.Opposite()] = null;
					mNeighbors[(int)i] = null;
				}
			}
		}
	}
}