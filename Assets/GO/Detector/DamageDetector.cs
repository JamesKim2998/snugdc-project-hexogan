using System;
using UnityEngine;

namespace HX
{
	public class DamageDetector : MonoBehaviour
	{
		public int owner;
		public float delay;
		public float delayLeft { get; private set; }

		public Action<Damage> onDetect { get; set; }

		void OnEnable()
		{
			delayLeft = 0;
		}

		void Update()
		{
			delayLeft -= Time.deltaTime;
		}

		public bool IsDamagable()
		{
			return enabled && delayLeft <= 0;
		}

		public void Damage(Damage attackData)
		{
			if (!IsDamagable())
				return;

			delayLeft = delay;

			if (onDetect != null)
				onDetect(attackData);
			else
				Debug.Log("postDamage is not set!");
		}

	}
}