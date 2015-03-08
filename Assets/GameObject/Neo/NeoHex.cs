using Gem;
using UnityEngine;

namespace HX
{
	public static class NeoHex
	{
		public static Vector2 Position(HexCoor _coor)
		{
			return (Vector2)_coor * NeoConst.HEX_P;
		}

		public static HexCoor Coor(Vector2 _position)
		{
			return HexCoor.Round(_position / NeoConst.HEX_P);
		}

		public static int Side(Vector2 _position, HexCoor _center)
		{
			return HexCoor.Side(_position / NeoConst.HEX_P, _center);
		}

		public static Vector2 Side(int _idx)
		{
			return (Vector2)HexCoor.FromAdjacent(_idx) / 2f * NeoConst.HEX_P;
		}

	}
}