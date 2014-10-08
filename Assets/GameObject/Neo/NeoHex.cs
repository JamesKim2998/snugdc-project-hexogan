using UnityEngine;
using System.Collections;

public static class NeoHex
{
	public static Vector2 Position(HexCoor _coor)
	{
		return _coor.ToVector2() * NeoConst.HEX_P;
	}

	public static HexCoor Coor(Vector2 _position, out int _side)
	{
		var _coor = HexCoor.Round(_position/NeoConst.HEX_P);
		_side = HexCoor.Side(_position, _coor);
		return _coor;
	}

	public static void Locate(Transform _transform, HexCoor _coor)
	{
		_transform.localPosition = Position(_coor);
	}

	public static Vector2 Side(int _idx)
	{
		return HexCoor.FromAdjacent(_idx).ToVector2() / 2f * NeoConst.HEX_P;
	}

	public static void LocateSide(Transform _transform, int _idx)
	{
		_transform.localPosition = Side(_idx);
		var _angles = _transform.localEulerAngles;
		_angles.z = 60 * _idx;
		_transform.localEulerAngles = _angles;
	}

}
