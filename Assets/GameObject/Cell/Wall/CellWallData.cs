using UnityEngine;
using System.Collections;

public class CellWallData : CellPartData, IDatabaseKey<CellWallType>
{
	public CellWallType type;
	public CellWallType Key() { return type; }

	public override string name_ { get { return type.ToString(); } }

	public new CellWall goPrf { get { return base.goPrf.GetComponent<CellWall>(); } }

	public int hp;

	public override GameObject MakeGO()
	{
		var _go = base.MakeGO();
		_go.GetComponent<CellWall>().Setup(this);
		return _go;
	}

	public CellWall MakeWall()
	{
		return MakeGO().GetComponent<CellWall>();
	}

}
