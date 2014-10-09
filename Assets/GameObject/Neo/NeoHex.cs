using UnityEngine;
using System.Collections;

public static class NeoHex
{
	public static Vector2 Position(HexCoor _coor)
	{
		return _coor.ToVector2() * NeoConst.HEX_P;
	}

	public static HexCoor Coor(Vector2 _position)
	{
		return HexCoor.Round(_position/NeoConst.HEX_P);
	}

	public static int Side(Vector2 _position, HexCoor _center)
	{
		return HexCoor.Side(_position/NeoConst.HEX_P, _center);
	}

	public static Vector2 Side(int _idx)
	{
		return HexCoor.FromAdjacent(_idx).ToVector2() / 2f * NeoConst.HEX_P;
	}

}
