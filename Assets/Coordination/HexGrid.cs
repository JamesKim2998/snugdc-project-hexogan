using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using UnityEngine;

public class HexGrid<T> : IEnumerable<KeyValuePair<HexCoor, HexCell<T>>>
{
	
	private readonly BiDictionary<HexCoor, HexCell<T>> m_Cells = new BiDictionary<HexCoor, HexCell<T>>();

	public HexCell<T> this[HexCoor _coor]
	{
		get { return Get(_coor); }
	}

	public bool TryGet(HexCoor _coor, out HexCell<T> _output)
	{
		return m_Cells.TryGetValue(_coor, out _output);
	}

	public HexCell<T> Get(HexCoor _coor)
	{
		HexCell<T> _output;
		if (TryGet(_coor, out _output))
		{
			return _output;
		}
		else
		{
			Debug.LogError("Cell is not exists for coor " + _coor + ". Return null.");
			return null;
		}
	}

	public bool Contains(HexCell<T> _cell)
	{
		return m_Cells.Reverse.ContainsKey(_cell);
	}

	public struct Neighbor
	{
		public int side;
		public HexCell<T> cell;
	}

	public IEnumerable<Neighbor> Neighbors(HexCoor _coor)
	{
		var _side = 0;
		foreach (var _coorNeighbor in _coor.Neighbors())
		{
			HexCell<T> _cell;
			if (TryGet(_coorNeighbor, out _cell))
				yield return new Neighbor{ cell = _cell, side = _side};
			++_side;
		}
	}

	public bool CheckAddable(HexCoor _coor, HexCell<T> _cell)
	{
		if (m_Cells.Count != 0)
		{
			HexCell<T> _temp;
			if (TryGet(_coor, out _temp))
			{
				Debug.LogError("There is already mechanic in " + _coor + ". Ignore.");
				return false;
			}

			if (Contains(_cell))
			{
				Debug.LogError("Trying to add mechanic. But already exists. Ignore.");
				return false;
			}

			if (!Neighbors(_coor).Any())
			{
				Debug.LogError("There is no neighbor around " + _coor + ". Ignore.");
				return false;
			}
		}

		return true;
	}

	public bool TryAdd(HexCoor _coor, HexCell<T> _cell)
	{
		if (CheckAddable(_coor, _cell))
		{
			Add(_coor, _cell);
			return true;
		}

		return false;
	}

	public void Add(HexCoor _coor, HexCell<T> _cell)
	{
		m_Cells.Add(_coor, _cell);
		foreach (var _neighbor in Neighbors(_coor))
			_cell.Connect(_neighbor.cell, _neighbor.side);
	}

	public void Remove(HexCoor _coor)
	{
		var _cell = Get(_coor);
		if (_cell == null) return;
		m_Cells.Remove(_coor);
		_cell.DisconnectAll();
	}

	public IEnumerator<KeyValuePair<HexCoor, HexCell<T>>> GetEnumerator()
	{
		return m_Cells.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return m_Cells.GetEnumerator();
	}
}
