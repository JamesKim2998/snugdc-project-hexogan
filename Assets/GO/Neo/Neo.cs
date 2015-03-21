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
			mechanics = gameObject.AddComponent<NeoMechanics>();
			mechanics.body = body;
		}

		void Start()
		{
			mechanics.Add(core.GetComponent<NeoBody>(), HexCoor.ZERO);
		}

		public void Motor(float _thrustNormal, float _driftNormal)
		{
			if (!mechanics) return;

			if (_thrustNormal > -0.01f)
				mechanics.motors.Motor(_thrustNormal, _driftNormal);
			else
				mechanics.motors.Motor(_thrustNormal, -_driftNormal);
		}

		public void Shoot()
		{
			if (!mechanics) return;
			mechanics.emitters.Shoot();
		}
	}
}