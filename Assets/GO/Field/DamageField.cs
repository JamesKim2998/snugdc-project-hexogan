using Gem;
using UnityEngine;

namespace HX
{
	public class DamageField : MonoBehaviour
	{
		public Damage damage;
		public LayerMask targetMask;

		void OnTriggerEnter2D(Collider2D _collider)
		{
			if (!enabled) return;

			if (targetMask.Contains(_collider))
			{
				var detector = _collider.GetComponent<DamageDetector>();
				if (detector) detector.Damage(damage);
			}
		}
	}
}