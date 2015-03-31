using Gem;
using UnityEngine;

namespace HX
{
	public class CellPlasmDB : DB<CellPlasmType, CellPlasmData>
	{
		private static CellPlasmDB sGlobal;
		public static CellPlasmDB g
		{
			get
			{
				if (sGlobal == null)
				{
#if UNITY_EDITOR
					sGlobal = Asset.Load<ScriptableObject>(new FullPath("Assets/DB/Cell/CellPlasmDB.asset")) as CellPlasmDB;
#endif
				}

				return sGlobal;
			}

			set { sGlobal = value; }
		}
	}
}