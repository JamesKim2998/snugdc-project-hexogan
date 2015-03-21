using Gem;
using UnityEngine;

namespace HX
{
	public class ProjectileDecoratorRicochet : MonoBehaviour
	{
		// penetration
		public int penetration;
		public LayerMask penetrationForced;
		private bool mIsPenetrating;
		private bool mIsPenetratingTemp;
		private float mPenetrationTimer;
		private int mPenetratingObject;

		// reflection
		public int reflectionCount;
		public float reflectRadius = 0.5f;
		public LayerMask reflectionMask;

		// effect
		public GameObject effectReflectPrf;
		public Vector3 effectReflectOffset;

		public GameObject effectPenetratePrf;
		public Vector3 effectPenetrateOffset;

		void Start()
		{
		}

		void Update()
		{
			mIsPenetrating = mIsPenetratingTemp;
			mIsPenetratingTemp = false;
		}

		void FixedUpdate()
		{
			if (mPenetrationTimer >= 0)
				mPenetrationTimer -= Time.fixedDeltaTime;

			TryReflect();
		}

		public bool ShouldCollide(Collider2D _collider)
		{
			if (mIsPenetrating || (mPenetratingObject == _collider.gameObject.GetInstanceID()))
				return false;

			if (mPenetrationTimer >= 0)
				return false;

			return true;
		}

		public bool OnCollision(Collider2D _collider)
		{
			var _shouldDecay = true;

			if (penetrationForced.Contains(_collider))
			{
				_shouldDecay = false;
				mIsPenetrating = true;
				mIsPenetratingTemp = true;
				mPenetrationTimer = 0.1f;
				mPenetratingObject = _collider.gameObject.GetInstanceID();

				if (effectPenetratePrf)
				{
					var _effect = (GameObject)Instantiate(effectPenetratePrf, transform.position, transform.rotation);
					_effect.transform.Translate(effectPenetrateOffset);
				}
			}
			else if (reflectionMask.Contains(_collider))
			{
				_shouldDecay = reflectionCount == 0;
			}

			return _shouldDecay;
		}

		bool TryReflect()
		{
			if (reflectionCount <= 0)
				return false;

			// raycast
			var _direction = transform.GetComponent<Rigidbody2D>().velocity.normalized;

			var _rayResult = Physics2D.Raycast(
				transform.position,
				_direction,
				reflectRadius,
				reflectionMask);

			if (_rayResult.collider)
			{
				// 너무 가까우면 fail.
				if ((_rayResult.point - (Vector2)transform.position).sqrMagnitude < 0.01f)
					return false;

				var _velocity = transform.GetComponent<Rigidbody2D>().velocity;
				var _rotation = Quaternion.FromToRotation(-_direction, _rayResult.normal);
				transform.GetComponent<Rigidbody2D>().velocity = _rotation * _rotation * -_velocity;
			}
			else
			{
				return false;
			}

			--reflectionCount;

			if (effectReflectPrf)
			{
				var _effect = (GameObject)Instantiate(effectReflectPrf, transform.position, transform.rotation);
				_effect.transform.Translate(effectReflectOffset);
			}

			return true;
		}
	}
}