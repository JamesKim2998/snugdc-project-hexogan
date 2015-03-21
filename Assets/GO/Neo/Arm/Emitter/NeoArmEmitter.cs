using Gem;
using UnityEngine;

namespace HX
{
	public class NeoArmEmitter : MonoBehaviour
	{
		public EmitterType emitterType;
		public Animator animator;

		public Emitter emitter { get; private set; }

		private void Start()
		{
			emitter = EmitterDB.g[emitterType].emitterPrf.Instantiate();
			emitter.transform.SetParentIdentity(transform);
			emitter.transform.localPosition = Vector3.zero;

			animator = GetComponent<Animator>();
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