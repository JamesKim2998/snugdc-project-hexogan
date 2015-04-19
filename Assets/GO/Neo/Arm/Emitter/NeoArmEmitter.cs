using Gem;
using UnityEngine;

namespace HX
{
	public class NeoArmEmitter : MonoBehaviour
	{
		[SerializeField] private NeoArm mArm;

		public EmitterType emitterType;

		public Emitter emitter { get; private set; }

		public Animator animator { get { return mArm.animator; } }

		private void Awake()
		{
			emitter = EmitterDB.g[emitterType].emitterPrf.Instantiate();
			emitter.transform.SetParent(transform, false);
			emitter.transform.localPosition = Vector3.zero;
		}

		public bool IsShootable()
		{
			return emitter.IsShootable();
		}

		public bool TryShoot()
		{
			if (! IsShootable())
				return false;

			Shoot();
			return true;
		}

		private void Shoot()
		{
			emitter.Shoot();
			animator.SetTrigger("shoot");
		}
	}
}