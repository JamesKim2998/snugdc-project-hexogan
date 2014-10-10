using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Neo : MonoBehaviour
{
	public NeoBodyCore core;

	public NeoRigidbody body;
	public NeoMechanics mechanics { get; private set; }

	void Awake()
	{
		mechanics = gameObject.AddComponent<NeoMechanics>();
		mechanics.body = body;
	}

	void Start()
	{
		mechanics.Add(core.GetComponent<NeoBody>(), HexCoor.ZERO);
	}

	public void Motor(float _thrustNormal, float _driftNormal)
	{
		if (_thrustNormal > -0.01f)
			mechanics.motors.Motor(_thrustNormal, _driftNormal);
		else
			mechanics.motors.Motor(_thrustNormal, -_driftNormal);
	}

	public void Shoot()
	{
		mechanics.emitters.Shoot();
	}
}
