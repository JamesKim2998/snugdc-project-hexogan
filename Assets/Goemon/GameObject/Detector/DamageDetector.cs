using System;
using UnityEngine;
using System.Collections;

public class DamageDetector : MonoBehaviour 
{
	public int owner;

	public Action<AttackData> postDamage { get; set; }

	public void Damage(AttackData attackData) {
		if (! enabled) return;

		if (postDamage != null) 
			postDamage(attackData);
		else
			Debug.Log("postDamage is not set!");
	}

}
