using UnityEngine;
using System.Collections;

public class NeoBody : NeoMechanic
{
	private readonly NeoMechanic[] m_Neighbors = new NeoMechanic[6];

	public NeoMechanic GetNeighbor(int _side)
	{
		return m_Neighbors[_side];
	}
	
	private void AddNeighbor(NeoMechanic _mechanic, int _side)
	{
		if (m_Neighbors[_side])
			Debug.LogError("There is already mechanic in side " + _side + ". Continue anyway.");
		m_Neighbors[_side] = _mechanic;
	}

	public void AddBody(NeoBody _body, int _side)
	{
		if (GetNeighbor(_side) == _body) 
			return;

		AddNeighbor(_body, _side);
		_body.AddBody(this, HexCoor.OppositeSide(_side));
	}

	public void AddArm(NeoArm _arm, int _side)
	{
		AddNeighbor(_arm, _side);
		_arm.Attach(this, _side);
	}
}
