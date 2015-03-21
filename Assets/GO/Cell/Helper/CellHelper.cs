using Gem;
using UnityEngine;

namespace HX
{
	public static class CellHelper
	{

		public static Cell MakeCell(CellPlasmType _plasmType, CellWallType _wallType = CellWallType.NONE)
		{
			var _cell = CellDB.g.cellPrf.Instantiate();

			var _plasm = CellPlasmDB.g[_plasmType].MakePlasm();
			_cell.plasm = _plasm;

			if (_wallType != CellWallType.NONE)
			{
				var _wall = CellWallDB.g[_wallType].MakeWall();
				_cell.wall = _wall;
			}

			return _cell;
		}

		public static CellPartDragAndDrop AddDragAndDrop(GameObject _cellPart)
		{
			if (_cellPart.GetComponent<CellPlasm>())
				return _cellPart.gameObject.AddComponent<CellPlasmDragAndDrop>();
			else if (_cellPart.GetComponent<CellWall>())
				return _cellPart.gameObject.AddComponent<CellWallDragAndDrop>();
			return null;
		}
	}
}