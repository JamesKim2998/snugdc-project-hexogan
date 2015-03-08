using UnityEngine;

namespace HX
{
	public static class LayerBits
	{
		public static readonly LayerMask NEO;
		public static readonly LayerMask CELL;
		public static readonly LayerMask VIRUS;
		public static readonly LayerMask PROJECTILE;

		static LayerBits()
		{
			NEO = 1 << LayerMask.NameToLayer("Neo");
			CELL = 1 << LayerMask.NameToLayer("Cell");
			VIRUS = 1 << LayerMask.NameToLayer("Virus");
			PROJECTILE = 1 << LayerMask.NameToLayer("Projectile");
		}
	}
}