using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class NeoMechanics : MonoBehaviour
{
	public NeoRigidbody body;
	public NeoArmMotors motors;

	private readonly HexGrid<NeoBody> m_Bodies = new HexGrid<NeoBody>();
	private readonly List<NeoArm> m_Arms = new List<NeoArm>();

	NeoMechanics()
	{
	}

	public void Add(NeoBody _body, HexCoor _coor)
	{
		m_Bodies.Add(_coor, new HexCell<NeoBody>(_body));
		_body.transform.parent = transform;
		NeoHex.Locate(_body.transform, _coor);
		body.AddMass(_body.mass, _coor * 2);
	}

	public void Add(NeoArm _arm, HexCoor _coor, int _side)
	{
		var _body = m_Bodies[_coor];
		if (_body == null) return;

		var _coorDouble = _coor * 2 + HexCoor.FromAdjacent(_side);

		_body.data.AddArm(_arm, _side);
		m_Arms.Add(_arm);
		body.AddMass(_arm.mass, _coorDouble);

		var _motor = _arm.GetComponent<NeoArmMotor>();
		if (_motor) motors.Add(_motor, _coor);
	}

	public void Build()
	{
		motors.BuildThrust();
	}
}
