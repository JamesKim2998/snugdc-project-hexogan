using Gem;
using TiledSharp;

namespace HX
{
	public struct LevelMarkerStartPosition
	{
		public HexCoor startPosition;

		public LevelMarkerStartPosition(HexCoor _coor, TilesetTile _tile)
		{
			startPosition = _coor;
		}

		public static implicit operator HexCoor(LevelMarkerStartPosition _this)
		{
			return _this.startPosition;
		}
	}

}