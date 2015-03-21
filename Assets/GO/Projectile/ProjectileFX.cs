using UnityEngine;

namespace HX
{
	public class ProjectileFX : MonoBehaviour
	{
		public GameObject fxHitPrf;
		public Vector3 fxHitOffset;

		void Start()
		{
			var _proj = GetComponent<Projectile>();
			_proj.onHit += OnHit;
		}

		void OnDestroy()
		{
			var _proj = GetComponent<Projectile>();
			_proj.onHit -= OnHit;
		}

		void OnHit(Projectile _proj, Collider2D _collider)
		{
			if (fxHitPrf)
			{
				var _effectHit = (GameObject)Instantiate(fxHitPrf, transform.position, transform.rotation);
				_effectHit.transform.Translate(fxHitOffset);
			}
		}
	}
}