using UnityEngine;

namespace HX
{
	[System.Serializable]
	public class Damage
	{
		public static readonly Damage MAX = new Damage(int.MaxValue);

		public string ownerPlayer;

		public EmitterType emitter;
		public ProjectileType projectile;

		[System.NonSerialized]
		public Vector2 velocity;
		public int value;

		public Damage(int _value)
		{
			value = _value;
		}

		public static implicit operator int(Damage _attackData)
		{
			return _attackData.value;
		}
	}
}