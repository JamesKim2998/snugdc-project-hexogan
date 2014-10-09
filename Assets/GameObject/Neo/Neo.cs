using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Neo : MonoBehaviour
{
	public NeoCore core;

	public NeoRigidbody body;
	public NeoMechanics mechanics { get; private set; }
	public NeoArmMotors motors { get; private set; }

	void Awake()
	{
		motors = gameObject.AddComponent<NeoArmMotors>();
		motors.body = body;

		mechanics = gameObject.AddComponent<NeoMechanics>();
		mechanics.body = body;
		mechanics.motors = motors;

		mechanics.Add(core.GetComponent<NeoBody>(), HexCoor.ZERO);
	}

	public void Motor(float _thrustNormal, float _driftNormal)
	{
		motors.Motor(_thrustNormal, _driftNormal);
	}
}
