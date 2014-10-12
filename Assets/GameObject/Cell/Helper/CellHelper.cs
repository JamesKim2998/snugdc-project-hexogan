using UnityEngine;
using System.Collections;

public static class CellHelper
{

	public static Cell MakeCell(CellPlasmType _plasmType, CellWallType _wallType = CellWallType.NONE)
	{
		var _cell = ComponentHelper.Instantiate(CellDatabase.cellPrf);

		var _plasm = CellPlasmDatabase.shared[_plasmType].MakeGO();
		_cell.plasm = _plasm;

		if (_wallType != CellWallType.NONE)
		{
			var _wall = CellWallDatabase.shared[_wallType].MakeGO();
			_cell.wall = _wall;
		}

		return _cell;
	}


}
