using System;
using System.Collections.Generic;
using Gem;
using HX.UI;
using UnityEngine;

namespace HX
{
	public class TheHexogan : MonoBehaviour
	{
		[NonSerialized]
		private static bool sIsInited;

		public EmitterDB emitterDB;
		public ProjectileDB projDB;
		public NeoConst neoConst;
		public NeoArmDB neoArmDB;
		public NeoBodyDB neoBodyDB;
		public CellDB cellDB;
		public CellPlasmDB cellPlasmDB;
		public CellWallDB cellWallDB;

		public UI.UIDB uiDB;

		void Awake()
		{
			if (!sIsInited)
			{
				sIsInited = true;
				Init();
			}
		}

		void Init()
		{
			EmitterDB.g = emitterDB;
			ProjectileDB.g = projDB;
			NeoConst.g = neoConst;
			NeoArmDB.g = neoArmDB;
			NeoBodyDB.g = neoBodyDB;
			CellDB.g = cellDB;
			CellPlasmDB.g = cellPlasmDB;
			CellWallDB.g = cellWallDB;
			UIDB.g = uiDB;
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