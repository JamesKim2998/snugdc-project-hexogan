using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{
	public class TheHexogan : MonoBehaviour
	{
		public EmitterDB emitterDB;
		public ProjectileDB projDB;
		public NeoConst neoConst;
		public NeoArmDB neoArmDB;
		public NeoBodyDB neoBodyDB;
		public CellDB cellDB;
		public CellPlasmDB cellPlasmDB;
		public CellWallDB cellWallDB;

		void Awake()
		{
			EmitterDB.g = emitterDB;
			ProjectileDB.g = projDB;
			NeoConst.g = neoConst;
			NeoArmDB.g = neoArmDB;
			NeoBodyDB.g = neoBodyDB;
			CellDB.g = cellDB;
			CellPlasmDB.g = cellPlasmDB;
			CellWallDB.g = cellWallDB;
		}

		public IEnumerable<IDB> GetDBs()
		{
			yield return emitterDB;
			yield return projDB;
			yield return neoArmDB;
			yield return neoBodyDB;
			yield return cellPlasmDB;
			yield return cellWallDB;
		}

#if UNITY_EDITOR
		public void Build()
		{
			foreach (var _db in GetDBs())
				_db.Build();
		}
#endif
	}
}