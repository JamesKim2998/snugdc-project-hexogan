using Gem;
using UnityEngine;

namespace HX
{
	public class Neo : MonoBehaviour
	{
		public NeoRigidbody body;
		public NeoMechanics mechanics { get; private set; }

		[SerializeField] private NeoBodyCore mCore;
		public NeoBodyCore core { get { return mCore; } }

		public readonly NeoRoughnessController roughness = new NeoRoughnessController();
		public readonly NeoEnergyController energy = new NeoEnergyController();

		void Awake()
		{
			mechanics = new NeoMechanics(this, roughness, energy);
		}

		void Start()
		{
			if (core)
			{
				var _parent = core.body.parent;
				if (!_parent) DoSetCore();	
			}
		}

		void Update()
		{
			mechanics.Update();
		}

		public void SetCore(NeoBodyCore _core)
		{
			D.Assert(mCore == null);
			mCore = _core;
			DoSetCore();
		}

		void DoSetCore()
		{
			mechanics.Add(core.body, HexCoor.ZERO);
		}

		public void Motor(float _dt, float _thrustNormal, float _driftNormal)
		{
			if (_thrustNormal > -0.01f)
				mechanics.motors.Motor(_dt, _thrustNormal, _driftNormal);
			else
				mechanics.motors.Motor(_dt, _thrustNormal, -_driftNormal);
		}

		public void Shoot()
		{
			mechanics.emitters.Shoot();
		}
	}
}