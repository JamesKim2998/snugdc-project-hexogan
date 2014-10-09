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

	private bool m_IsDirty = false;

	NeoMechanics()
	{
	}

	void Update()
	{
		if (m_IsDirty)
		{
			Build();
			m_IsDirty = false;
		}
	}

	public HexCell<NeoBody> GetBody(HexCoor _coor)
	{
		HexCell<NeoBody> _body;
		return m_Bodies.TryGet(_coor, out _body)
			? _body
			: null;
	}

	void Add(NeoMechanic _mechanic)
	{
		_mechanic.mechanics = this;
		m_IsDirty = true;
	}

	public bool Add(NeoBody _body, HexCoor _coor)
	{
		var _cell = new HexCell<NeoBody>(_body);
		if (!m_Bodies.TryAdd(_coor, _cell))
			return false;

		Add(_body);

		_body.transform.parent = transform;
		Locate(_body.transform, _coor);
		body.AddMass(_body.mass, _coor * 2);

		var _side = -1;
		foreach (var _neighbor in _cell.GetNeighbors())
		{
			++_side;
			if (_neighbor == null) continue;
			_body.AddBody(_neighbor.data, _side);
		}
		
		return true;
	}

	public void Add(NeoArm _arm, HexCoor _coor, int _side)
	{
		var _body = m_Bodies[_coor];
		if (_body == null) return;

		Add(_arm);

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

	public static void Locate(Transform _transform, HexCoor _coor)
	{
		var _pos = (Vector3)NeoHex.Position(_coor);
		var _posOld = _transform.localPosition;
		_pos.z = _posOld.z;
		_transform.localPosition = _pos;
	}

}
