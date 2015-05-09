using Gem;
using UnityEngine;	

namespace HX
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Projectile : MonoBehaviour
	{
		public ProjectileType type;

		[HideInInspector]
		public int owner = 0;

		#region active

		private int mActivateCounter = 0;
		public bool activated { get { return mActivateCounter >= 0; } }

		public bool Activate()
		{
#if DEBUG
			if (mActivateCounter == 0)
				Debug.LogWarning("Activate counter should not be greater than 0. ");
#endif
			return ++mActivateCounter == 0;
		}

		public bool Deactivate() { return mActivateCounter-- == 0; }

		#endregion

		#region life cycle
		// life
		public float life = 10;
		private float mAge = 0;

		// prepare
		public float prepareDuration = 0;
		private float mPrepareTime;

		// decay
		public bool decaying { get; private set; }
		public bool stopOnDecay = true;
		public float decayDuration = 0;
		private float mDecayTime;
		#endregion

		#region attack
		public bool isHitOwner = false;

		[HideInInspector]
		public Damage damage;
		#endregion

		// physics
		public Vector2 initialVelocity = Vector2.zero;
		public Vector2 drivingForce = Vector2.zero;
		public Vector2 relativeDrivingForce = Vector2.zero;

		// filter
		public LayerMask collisionIgnores;
		public LayerMask collisionTargets;
		public LayerMask collisionTerrains;

		// components
		private ProjectileDecoratorRicochet mRicochet;
		private Animator mAnimator;

		// collide with something
		public delegate void PostCollide(Projectile _projectile, Collider2D _collider);
		public event PostCollide onCollide;

		// collide with targets
		public delegate void PostHit(Projectile _projectile, Collider2D _collider);
		public event PostHit onHit;

		// collide with terrains
		public delegate void PostBumped(Projectile _projectile, Collider2D _collider);
		public event PostBumped onBumped;

		public Projectile()
		{
			decaying = false;
		}

		void Start()
		{
			if (initialVelocity != Vector2.zero)
				GetComponent<Rigidbody2D>().velocity = initialVelocity;

			mRicochet = GetComponent<ProjectileDecoratorRicochet>();
			mAnimator = GetComponent<Animator>();

			if (!prepareDuration.Equals(0))
			{
				mPrepareTime = 0;
				Deactivate();
			}
		}

		void DestroySelf()
		{
			if (GetComponent<NetworkView>() && GetComponent<NetworkView>().enabled && GetComponent<NetworkView>().viewID != NetworkViewID.unassigned)
				Network.Destroy(GetComponent<NetworkView>().viewID);
			else
				Destroy(gameObject);
		}

		void StartDecay()
		{
			if (decaying) return;

			decaying = true;

			if (decayDuration.Equals(0))
			{
				DestroySelf();
				return;
			}

			mDecayTime = 0;

			if (stopOnDecay)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				GetComponent<Rigidbody2D>().isKinematic = true;
			}
		}

		void Update()
		{
			var dt = Time.deltaTime;

			if (!activated)
			{
				if (mPrepareTime < prepareDuration)
				{
					mPrepareTime += dt;

					if (mPrepareTime >= prepareDuration)
					{
						Activate();
						if (mAnimator != null)
							mAnimator.SetTrigger("Activate");
					}
				}
			}

			if (activated)
			{

				if (mAge < life)
				{
					mAge += dt;

					if (mAge >= life)
						StartDecay();
				}
			}

			if (decaying)
			{
				mDecayTime += dt;
				if (mDecayTime > decayDuration)
					DestroySelf();
			}
		}

		void FixedUpdate()
		{
			if (!decaying)
			{
				if (!GetComponent<Rigidbody2D>().isKinematic)
				{
					Vector2 _drivingForce = Vector2.zero;

					if (!drivingForce.Equals(Vector2.zero))
						_drivingForce += drivingForce;

					if (!relativeDrivingForce.Equals(Vector2.zero))
					{
						var _drivingForceWorld = transform.localToWorldMatrix.MultiplyVector(relativeDrivingForce);
						_drivingForce += new Vector2(_drivingForceWorld.x, _drivingForceWorld.y);
					}

					if (_drivingForce != Vector2.zero)
						GetComponent<Rigidbody2D>().AddForce(_drivingForce);
				}
			}
		}

		void OnCollisionEnter2D(Collision2D _collision)
		{
			OnCollision(_collision.collider);
		}

		void OnTriggerEnter2D(Collider2D _collider)
		{
			OnCollision(_collider);
		}

		void OnCollision(Collider2D _collider)
		{
			if (decaying) return;

			if (collisionIgnores.Contains(_collider)) return;

			if (mRicochet)
			{
				if (!mRicochet.ShouldCollide(_collider))
					return;

			}

			if (collisionTargets.Contains(_collider))
			{
				if (!activated)
					goto finalize;

				var _damageDetector = _collider.GetComponentInChildren<DamageDetector>();
				if (!_damageDetector) return;

				var _isOwner = _damageDetector.owner == owner;

				if (!isHitOwner && _isOwner)
					return;

				if (_damageDetector && _damageDetector.enabled)
					_damageDetector.Damage(damage);

				if (onHit != null)
					onHit(this, _collider);
			}
			else if (collisionTargets.Contains(_collider.gameObject))
			{
				if (onBumped != null)
					onBumped(this, _collider);
			}

		finalize:

			if (onCollide != null)
				onCollide(this, _collider);

			if (mRicochet)
			{
				if (mRicochet.OnCollision(_collider))
					StartDecay();
			}
			else
			{
				StartDecay();
			}
		}


	}
}