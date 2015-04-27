using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{
	public class TheHexogan : MonoBehaviour
	{
		[NonSerialized]
		private static bool sIsInited;

		public GODB goDB;
		public EmitterDB emitterDB;
		public ProjectileDB projDB;
		public NeoConst neoConst;
		public NeoDB neoDB;
		public NeoArmDB neoArmDB;
		public NeoBodyDB neoBodyDB;
		public CellDB cellDB;
		public CellPlasmDB cellPlasmDB;
		public CellWallDB cellWallDB;

		public UI.UIDB uiDB;
		public Stage.UIDB stageUIDB;

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
			GODB.g = goDB;
			EmitterDB.g = emitterDB;
			ProjectileDB.g = projDB;
			NeoConst.g = neoConst;
			NeoDB.g = neoDB;
			NeoArmDB.g = neoArmDB;
			NeoBodyDB.g = neoBodyDB;
			CellDB.g = cellDB;
			CellPlasmDB.g = cellPlasmDB;
			CellWallDB.g = cellWallDB;
			UI.UIDB.g = uiDB;
			Stage.UIDB.g = stageUIDB;
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