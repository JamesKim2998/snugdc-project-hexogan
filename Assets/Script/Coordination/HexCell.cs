using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HexCell<T>
{
	public T data { get; private set; }
	private readonly HexCell<T>[] m_Neighbors = new HexCell<T>[6];

	public HexCell(T _data)
	{
		data = _data;
	}

	~HexCell()
	{
		for (var _i = 0; _i < 6; ++_i)
		{
			var _neighbor = m_Neighbors[_i];
			if (_neighbor != null) _neighbor.m_Neighbors[HexCoor.OppositeSide(_i)] = null;
		}
	}

	public HexCell<T> GetNeighbor(int _idx)
	{
		return m_Neighbors[_idx];
	}

	public IEnumerable<HexCell<T>> GetNeighbors()
	{
		for (var _i = 0; _i < 6; ++_i)
			yield return m_Neighbors[_i];
	}

	public void Connect(HexCell<T> _neighbor, int _idx)
	{
		if (m_Neighbors[_idx] != null)
		{
			Debug.LogError("There is already mechanic exists in " + _idx + ". Ignore.");
			return;
		}

		m_Neighbors[_idx] = _neighbor;
		_neighbor.m_Neighbors[HexCoor.OppositeSide(_idx)] = this;
	}

	public void Disconnect(int _idx)
	{
		if (m_Neighbors[_idx] == null)
		{
			Debug.LogWarning("There is no neighbor in " + _idx + ". Ignore.");
			return;
		}

		var _neighbor = m_Neighbors[_idx];
		_neighbor.m_Neighbors[HexCoor.OppositeSide(_idx)] = null;
		m_Neighbors[_idx] = null;
	}

}
