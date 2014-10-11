using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Neo))]
[RequireComponent(typeof(Rigidbody2D))]
public class NeoRigidbody : MonoBehaviour
{
	public float speedLimit = 3;
	public float angularSpeedLimit = 720;

	void Awake()
	{
		rigidbody2D.mass = 0;
		rigidbody2D.inertia = 0;
	}

	public void Update()
	{
		if (m_IsMassDirty)
			BuildMass();
	}

	class MassData
	{
		public int mass;
	}

	private bool m_IsMassDirty = false;
	private readonly Dictionary<HexCoor, MassData> m_MassDatas = new Dictionary<HexCoor, MassData>();
	private int m_TotalMass;
	private HexCoor m_MassWeightedDoubleCoor;

	public void AddMass(int _mass, HexCoor _coor, int? _side = null)
	{
		m_IsMassDirty = true;

		var _coorDouble = _coor * 2;
		if (_side.HasValue) _coorDouble += HexCoor.FromAdjacent(_side.Value);

		m_TotalMass += _mass;
		m_MassWeightedDoubleCoor += _coorDouble * _mass;

		MassData _massData;
		if (m_MassDatas.TryGetValue(_coorDouble, out _massData))
			_massData.mass += _mass;
		else
			m_MassDatas.Add(_coorDouble, new MassData { mass = _mass });
	}

	public void BuildMass()
	{
		if (! m_IsMassDirty) return;
		m_IsMassDirty = false;
		
		rigidbody2D.mass = m_TotalMass;
		rigidbody2D.centerOfMass = NeoHex.Position(m_MassWeightedDoubleCoor) / 2 / m_TotalMass;

		float _inertia = 0;
		foreach (var _massData in m_MassDatas)
			_inertia += _massData.Value.mass*((Vector2) _massData.Key / 2).sqrMagnitude;
		rigidbody2D.inertia = _inertia;
	}

	public static implicit operator Rigidbody2D(NeoRigidbody _self)
	{
		return _self.rigidbody2D;
	}
}
