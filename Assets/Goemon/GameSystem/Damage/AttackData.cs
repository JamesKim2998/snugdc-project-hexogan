using UnityEngine;

[System.Serializable]
public class AttackData 
{
	public static readonly AttackData DAMAGE_MAX = new AttackData(int.MaxValue);

	public string ownerPlayer;

	public EmitterType emitter = EmitterType.NONE;
    public ProjectileType projectile = ProjectileType.NONE;

	[System.NonSerialized]
	public Vector2 velocity = Vector2.zero;
	public int damage = 0;

	public AttackData(int _damage)
	{
		damage = _damage;
	}

	public static implicit operator int(AttackData _attackData)
	{
		return _attackData.damage;
	}
}
