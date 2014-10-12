using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections;

public class CellWall : MonoBehaviour
{
	public CellWallData data { get; private set; }

	public int hpLeft { get; private set; }
	public Action<CellWall> postDecay;

	public DamageDetector damageDetector;

	public void Start()
	{
		if (!damageDetector)
			damageDetector = gameObject.AddComponent<DamageDetector>();

		damageDetector.postDamage += Damage;
	}

	public void OnDestroy()
	{
		if (damageDetector)
			damageDetector.postDamage -= Damage;
	}

	#region data

	public Action<CellWall, CellWallData> postDataSetuped;

	public void Setup(CellWallData _data)
	{
		if (data)
		{
			Debug.LogWarning("Trying to setup again. Continue anyway.");
			return;
		}

		data = _data;

		hpLeft = data.hp;

		if (postDataSetuped != null) postDataSetuped(this , _data);
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

	private void Damage(AttackData _attackData)
	{
		hpLeft -= _attackData.damage;
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
