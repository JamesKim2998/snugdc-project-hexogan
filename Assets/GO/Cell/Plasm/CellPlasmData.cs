using Gem;
using UnityEngine;

namespace HX
{
	public class CellPlasmData : CellPartData, IDBKey<CellPlasmType>
	{
		public CellPlasmType key { get; set; }
		public override string name { get { return key.ToString(); } }

		public int hp;

		public override GameObject MakeGO()
		{
			return MakePlasm().gameObject;
		}

		public CellPlasm MakePlasm()
		{
			var _go = base.MakeGO();
			var _plasm = _go.GetComponent<CellPlasm>();
			_plasm.Setup(this);
			return _plasm;
		}
	}
}