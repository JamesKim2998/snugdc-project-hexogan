using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(Emitter))]
	public class EmitterDecoratorDrivingForce : MonoBehaviour
	{
		private Emitter mEmitter;
		public float drivingForce;
		public float explosionRadius;

		void Awake()
		{
			mEmitter = GetComponent<Emitter>();
			mEmitter.doShoot += DoShoot;
		}

		void OnDestroy()
		{
			mEmitter.doShoot -= DoShoot;
		}

		void DoShoot(Emitter _emitter, GameObject _proj)
		{
			_proj.GetComponent<Projectile>().drivingForce = drivingForce * _emitter.transform.right;
		}
	}
}