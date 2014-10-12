using UnityEngine;
using System.Collections;

public class CellPlasmData : MonoBehaviour, IDatabaseKey<CellPlasmType>
{
	public CellPlasmType type;
	public CellPlasmType Key() { return type; }

	public CellPlasm goPrf;

	public int hp;

	public CellPlasm MakeGO()
	{
		var _go = ComponentHelper.Instantiate(goPrf);
		_go.Setup(this);
		return _go;
	}
}
