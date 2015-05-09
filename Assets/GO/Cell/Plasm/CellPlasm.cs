using System;
using UnityEngine;

namespace HX
{
	public class CellPlasm : MonoBehaviour
	{
		public CellPlasmData data { get; private set; }

		public float hpLeft { get; private set; }
		public Action<CellPlasm> postDecay;

		public DamageDetector damageDetector;

		public void Awake()
		{
			if (! damageDetector)
				damageDetector = gameObject.AddComponent<DamageDetector>();

			damageDetector.onDetect += Damage;
		}

		public void OnDestroy()
		{
			if (damageDetector)
				damageDetector.onDetect -= Damage;
		}

		public void Setup(CellPlasmData _data)
		{
			if (data)
			{
				Debug.LogWarning("Trying to setup again. Continue anyway.");
				return;
			}

			data = _data;

			hpLeft = data.hp;
		}

		#region life

		public bool IsDamagable()
		{
			return damageDetector.IsDamagable();
		}

		public void SetDamagable(bool _var)
		{
			damageDetector.enabled = _var;
		}

		private void Damage(Damage _attackData)
		{
			hpLeft -= _attackData.value;
			if (hpLeft <= 0) Decay();
		}

		private void Decay()
		{
			SetDamagable(false);
			Destroy(gameObject, CellConst.PLASM_DECAY_DELAY);
			if (postDecay != null) postDecay(this);
		}

		#endregion
	}

}