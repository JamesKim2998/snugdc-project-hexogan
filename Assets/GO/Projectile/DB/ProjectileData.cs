using Gem;
using UnityEngine;

namespace HX
{
	public class ProjectileData : ScriptableObject, IDBKey<ProjectileType>
	{
		public ProjectileType key { get; set; }

		public Projectile projectilePrf;
	}

}