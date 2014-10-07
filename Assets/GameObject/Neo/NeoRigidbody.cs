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
		rigidbody2D.mass = 1;
		rigidbody2D.inertia = 1;
	}

	private int m_TotalMass = 0;
	private HexCoor m_MassWeightedDoubleCoor = new HexCoor();

	public void AddMass(int _mass, HexCoor _coorDouble)
	{
		m_TotalMass += _mass;
		m_MassWeightedDoubleCoor += _coorDouble * _mass;
		rigidbody2D.mass = m_TotalMass;
		rigidbody2D.centerOfMass = NeoHex.Position(m_MassWeightedDoubleCoor) / 2 / m_TotalMass;
	}

}
