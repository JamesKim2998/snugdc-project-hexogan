using UnityEngine;
using System.Collections;

public class NeoBody : NeoMechanic
{
	private readonly NeoMechanic[] m_Mechanics = new NeoMechanic[6];

	private void AddMechnic(NeoMechanic _mechanic, int _side)
	{
		if (m_Mechanics[_side])
			Debug.LogError("There is already mechanic in side " + _side + ". Continue anyway.");
		m_Mechanics[_side] = _mechanic;
	}

	public void AddArm(NeoArm _arm, int _side)
	{
		AddMechnic(_arm, _side);
		_arm.Attach(this, _side);
	}
}
