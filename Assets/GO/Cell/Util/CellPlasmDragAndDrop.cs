using Gem;

namespace HX
{
	public class CellPlasmDragAndDrop : CellPartDragAndDrop
	{
		protected override bool IsLocatable(HexNode<Cell> _cell, HexCoor _coor)
		{
			if (!base.IsLocatable(_cell, _coor))
				return false;

			return _cell == null || !_cell.data.plasm;
		}

		protected override bool Attach(HexNode<Cell> _cell, HexCoor _coor)
		{
			if (_cell != null && _cell.data.plasm)
				return false;

			if (_cell == null)
			{
				var _cellGO = cellPrf.Instantiate();
				_cell = cellGrid.Add(_cellGO, _coor);
			}

			_cell.data.plasm = GetComponent<CellPlasm>();

			return true;
		}
	}
}