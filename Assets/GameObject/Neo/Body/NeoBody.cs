using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NeoBody : NeoMechanic
{
	public NeoBodyType type;

	private readonly NeoMechanic[] m_Neighbors = new NeoMechanic[6];

	public NeoMechanic GetNeighbor(int _side)
	{
		return m_Neighbors[_side];
	}

	public IEnumerator<NeoMechanic> GetNeighbors()
	{
		for (int _side = 0; _side != 6; ++_side)
			yield return GetNeighbor(_side);
	}

	private void SetNeighbor(int _side, NeoMechanic _mechanic)
	{
		if (GetNeighbor(_side) && _mechanic)
		{
			Debug.LogError("There is already mechanic in side " + _side + ". Ignore.");
			return;
		}

		m_Neighbors[_side] = _mechanic;
	}

	public void AddBody(NeoBody _body, int _side)
	{
		if (GetNeighbor(_side) == _body) 
			return;

		SetNeighbor(_side, _body);
		_body.AddBody(this, HexCoor.OppositeSide(_side));
	}

	public void AddArm(NeoArm _arm, int _side)
	{
		SetNeighbor(_side, _arm);
		_arm.Attach(this, _side);
	}

	public void RemoveNeighbor(int _side)
	{
		var _neighbor = GetNeighbor(_side);
		if (! _neighbor) return;

		SetNeighbor(_side, null);

		var _body = _neighbor.GetComponent<NeoBody>();
		if (_body) _body.RemoveNeighbor(HexCoor.OppositeSide(_side));

		var _arm = _neighbor.GetComponent<NeoArm>();
		if (_arm) parent.Remove(_arm);
	}

	public override void Detach()
	{
		for (var _side = 0; _side != 6; ++_side)
			RemoveNeighbor(_side);
		base.Detach();
	}
}
