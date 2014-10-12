using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	#region plasm
	private CellPlasm m_Plasm;
	public CellPlasm plasm
	{
		get { return m_Plasm;  }
		set
		{
			if (plasm == value) 
				return;

			if (plasm && value)
			{
				Debug.LogError("You cannot change plasm. Ignore.");
				return;
			}

			var _plasmOld = m_Plasm;
			m_Plasm = value;

			if (plasm)
			{
				TransformHelper.SetParentWithoutScale(plasm, this);
				plasm.transform.localPosition = Vector3.zero;
				plasm.postDecay += ListenPlasmDecay;
				if (wall) plasm.SetDamagable(false);
			}
			else
			{
				_plasmOld.postDecay -= ListenPlasmDecay;
			}
		}
	}

	private void ListenPlasmDecay(CellPlasm _plasm)
	{
		plasm = null;
		Destroy(this, CellConst.CELL_DECAY_DELAY);
	}
	#endregion

	#region wall
	private CellWall m_Wall;
	public CellWall wall
	{
		get { return m_Wall; }
		set
		{
			if (wall == value)
				return;

			if (wall && value)
			{
				Debug.LogError("You cannot change wall. Ignore.");
				return;
			}

			var _wallOld = m_Wall;
			m_Wall = value;

			if (wall)
			{
				TransformHelper.SetParentWithoutScale(wall, this);
				wall.transform.localPosition = Vector3.zero;
				wall.postDecay += ListenWallDecay;
				if (plasm) plasm.SetDamagable(false);
			}
			else
			{
				_wallOld.postDecay -= ListenWallDecay;
				if (plasm) plasm.SetDamagable(true);
			}
		}
	}

	private void ListenWallDecay(CellWall _wall)
	{
		wall = null;
	}
	#endregion

	public CellMembrain membrain;

	#region parent
	public CellGrid grid { get; private set; }

	public bool SetGrid(CellGrid _grid)
	{
		if (grid)
		{
			Debug.LogWarning("Already grid exists! Ignore.");
			return false;
		}

		if (!_grid)
		{
			Debug.LogWarning("Trying to set null grid! Ignore.");
			return false;
		}

		grid = _grid;

		return true;
	}

	public void DetachGrid()
	{
		grid = null;
	}
	#endregion

	#region neighbor
	private readonly Cell[] m_Neighbors = new Cell[6];

	public Cell GetNeighbor(int _side)
	{
		return m_Neighbors[_side];
	}

	public IEnumerator<Cell> GetNeighbors()
	{
		for (int _side = 0; _side != 6; ++_side)
			yield return GetNeighbor(_side);
	}

	public void SetNeighbor(Cell _cell, int _side)
	{
		var _neighbor = GetNeighbor(_side);
		if (_neighbor && _cell)
		{
			Debug.LogError("There is already mechanic in side " + _side + ". Ignore.");
			return;
		}

		m_Neighbors[_side] = _cell;
	}

	public void RemoveNeighbor(int _side)
	{
		var _neighbor = GetNeighbor(_side);
		if (!_neighbor) return;

		SetNeighbor(null, _side);
	}
	#endregion

}
