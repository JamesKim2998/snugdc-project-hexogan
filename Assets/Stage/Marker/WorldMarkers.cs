using Gem;
using TiledSharp;

namespace HX
{
	public struct WorldMarkerStartPosition
	{
		public HexCoor startPosition;

		public WorldMarkerStartPosition(HexCoor _coor, TilesetTile _tile)
		{
			startPosition = _coor;
		}

		public static implicit operator HexCoor(WorldMarkerStartPosition _this)
		{
			return _this.startPosition;
		}
	}

}