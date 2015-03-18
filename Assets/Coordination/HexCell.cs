using System.Collections.Generic;
using UnityEngine;

namespace Gem
{
	public class HexCell<T>
	{
		public T data { get; private set; }
		private readonly HexCell<T>[] mNeighbors = new HexCell<T>[6];

		public HexCell(T _data)
		{
			data = _data;
		}

		~HexCell()
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

		public HexCell<T> GetNeighbor(HexIdx _idx)
		{
			return mNeighbors[(int)_idx];
		}

		public IEnumerable<HexCell<T>> GetNeighbors()
		{
			for (var _i = 0; _i != 6; ++_i)
				yield return mNeighbors[_i];
		}

		public void Connect(HexCell<T> _neighbor, HexIdx _idx)
		{
			if (GetNeighbor(_idx) != null)
			{
				Debug.LogError("There is already mechanic exists in " + _idx + ". Ignore.");
				return;
			}

			mNeighbors[(int)_idx] = _neighbor;
			_neighbor.mNeighbors[(int)_idx.Opposite()] = this;
		}

		public void Disconnect(HexIdx _idx)
		{
			if (GetNeighbor(_idx) == null)
			{
				Debug.LogWarning("There is no neighbor in " + _idx + ". Ignore.");
				return;
			}

			var _neighbor = GetNeighbor(_idx);
			_neighbor.mNeighbors[(int)_idx.Opposite()] = null;
			mNeighbors[(int)_idx] = null;
		}

		public void DisconnectAll()
		{
			foreach (var i in HexHelper.GetIdxes())
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