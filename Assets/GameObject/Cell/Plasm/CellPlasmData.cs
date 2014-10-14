using UnityEngine;
using System.Collections;

public class CellPlasmData : CellPartData, IDatabaseKey<CellPlasmType>
{
	public CellPlasmType type;
	public CellPlasmType Key() { return type; }

	public override string name_ { get { return type.ToString(); } }

	public new CellPlasm goPrf { get { return base.goPrf.GetComponent<CellPlasm>(); } }

	public int hp;

	public override GameObject MakeGO()
	{
		var _go = base.MakeGO();
		_go.GetComponent<CellPlasm>().Setup(this);
		return _go;
	}

	public CellPlasm MakePlasm()
	{
		return MakeGO().GetComponent<CellPlasm>();
	}
}
