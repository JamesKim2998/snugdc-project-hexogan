using UnityEngine;

namespace HX
{
	public class ProjectileDecoratorExplosion : MonoBehaviour
	{
		private Projectile mProjectile;
		public Explosion explosionPrf;
		private bool mExploded = false;

		void Start()
		{
			mProjectile = GetComponent<Projectile>();
			mProjectile.onHit += Explode;
			mProjectile.onBumped += Explode;
		}

		void OnDestroy()
		{
			mProjectile.onHit -= Explode;
			mProjectile.onBumped -= Explode;
		}

		void Explode(Projectile _projectile, Collider2D _collider)
		{
			if (mExploded)
			{
				Debug.Log("Trying to explode multiple times. Ignore.");
				return;
			}

			ExplodeOnCrash_RequestExplode(transform.position);

			if (Network.peerType == NetworkPeerType.Server)
				GetComponent<NetworkView>().RPC("ExplodeOnCrash_RequestExplode", RPCMode.Others, transform.position);
		}

		[RPC]
		void ExplodeOnCrash_RequestExplode(Vector3 _position)
		{
			if (mExploded) return;

			mExploded = true;

			var _explosionGO = (GameObject)Instantiate(explosionPrf.gameObject, _position, Quaternion.identity);

			var _field = _explosionGO.GetComponent<DamageField>();
			_field.damage = mProjectile.damage;
			_field.targetMask = mProjectile.collisionTargets;

			var _explosion = _explosionGO.GetComponent<Explosion>();
			_explosion.Explode();
		}

	}
}