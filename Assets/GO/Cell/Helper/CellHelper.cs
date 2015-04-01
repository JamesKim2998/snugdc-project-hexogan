using Gem;

namespace HX
{
	public static class CellHelper
	{

		public static Cell MakeCell(CellData _data)
		{
			var _cell = CellDB.g.cellPrf.Instantiate();

			var _plasm = CellPlasmDB.g[_data.plasm].MakePlasm();
			_cell.plasm = _plasm;

			if (_data.wall != CellWallType.NONE)
			{
				var _wall = CellWallDB.g[_data.wall].MakeWall();
				_cell.wall = _wall;
			}

			return _cell;
		}
	}
}