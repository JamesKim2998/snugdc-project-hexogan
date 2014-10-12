using UnityEngine;
using System.Collections;

public class CellWallData : MonoBehaviour, IDatabaseKey<CellWallType>
{
	public CellWallType type;
	public CellWallType Key() { return type; }

	public CellWall goPrf;

	public int hp;

	public CellWall MakeGO()
	{
		var _go = ComponentHelper.Instantiate(goPrf);
		_go.Setup(this);
		return _go;
	}
}
