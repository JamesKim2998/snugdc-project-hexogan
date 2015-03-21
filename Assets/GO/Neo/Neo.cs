using Gem;
using UnityEngine;

namespace HX
{
	public class Neo : MonoBehaviour
	{
		public NeoBodyCore core;

		public NeoRigidbody body;
		public NeoMechanics mechanics { get; private set; }

		void Awake()
		{
			mechanics = new NeoMechanics(this);
		}

		void Start()
		{
			mechanics.Add(core.body, HexCoor.ZERO);
		}

		void Update()
		{
			mechanics.Update();
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
}