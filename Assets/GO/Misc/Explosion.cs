using UnityEngine;

namespace HX
{
	public class Explosion : MonoBehaviour
	{
		public CircleCollider2D field;

		public bool executeOnStart = true;

		public float radius;
		public float duration;

		public float impulse;
		public LayerMask impulseMask;
		public float impulseRadius;

		public GameObject scaleTarget;

		private bool mExploded = false;
		private float mExplosionTime = 0;

		void Start()
		{
			if (field)
				field = GetComponent<Collider2D>() as CircleCollider2D;

			if (executeOnStart)
				Explode();
		}

		void Update()
		{
			if (!mExploded) return;
			mExplosionTime += Time.deltaTime;

			var _radius = radius * mExplosionTime / duration;
			field.radius = _radius;
			if (scaleTarget) scaleTarget.transform.localScale = new Vector3(_radius, _radius, 1);
		}

		void Impulse()
		{
			var _rayResults = Physics2D.OverlapCircleAll(transform.position, impulseRadius, impulseMask);
			foreach (var _rayResult in _rayResults)
			{
				if (!_rayResult.GetComponent<Rigidbody2D>()) continue;
				var _delta = _rayResult.transform.position - transform.position;
				var _distance = _delta.magnitude;
				var _direction = _delta / _distance;
				var _factor = (_distance < radius) ? 1.0f
					: (impulseRadius - _distance) / (impulseRadius - radius);
				var _impulse = impulse * _factor * _direction;
				_rayResult.GetComponent<Rigidbody2D>().AddForce(_impulse, ForceMode2D.Impulse);
			}
		}

		public void Explode()
		{
			if (mExploded)
			{
				Debug.LogWarning("Trying to explode again. Ignore.");
				return;
			}

			mExploded = true;
			Invoke("Impulse", duration / 2);
			Invoke("DestroySelf", duration);
		}

		void DestroySelf()
		{
			Destroy(gameObject);
		}
	}
}