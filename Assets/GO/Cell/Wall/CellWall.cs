using System;
using UnityEngine;

namespace HX
{
	public class CellWall : MonoBehaviour
	{
		public CellWallData data { get; private set; }

		public float hpLeft { get; private set; }
		public Action<CellWall> postDecay;

		public DamageDetector damageDetector;

		public void Start()
		{
			if (!damageDetector)
				damageDetector = gameObject.AddComponent<DamageDetector>();

			damageDetector.onDetect += Damage;
		}

		public void OnDestroy()
		{
			if (damageDetector)
				damageDetector.onDetect -= Damage;
		}

		#region data

		public Action<CellWall, CellWallData> onDataSetuped;

		public void Setup(CellWallData _data)
		{
			if (data)
			{
				Debug.LogWarning("Trying to setup again. Continue anyway.");
				return;
			}

			data = _data;

			hpLeft = data.hp;

			if (onDataSetuped != null) onDataSetuped(this, _data);
		}
		#endregion

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
			Destroy(gameObject, CellConst.WALL_DECAY_DELAY);
			if (postDecay != null) postDecay(this);
		}
		#endregion
	}
}