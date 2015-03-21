using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(Emitter))]
	public class SimpleLauncherDef : MonoBehaviour
	{
		public ProjectileType projectile;

		private Projectile mProjectilePrf;
		public Projectile projectilePrf
		{
			get
			{
				if (mProjectilePrf) return mProjectilePrf;
				mProjectilePrf = ProjectileDB.g[projectile].projectilePrf;
				return mProjectilePrf;
			}
		}

		void Start()
		{
			var _emitter = GetComponent<Emitter>();

#if DEBUG
			if (_emitter == null)
			{
				Debug.LogError("Emitter is not exists! Ignore.");
				return;
			}
#endif

			_emitter.doCreateProjectile = delegate { return (GameObject)Instantiate(projectilePrf.gameObject); };

			_emitter.doCreateProjectileServer = delegate { return (GameObject)Instantiate(projectilePrf.gameObject); };
		}

	}
}