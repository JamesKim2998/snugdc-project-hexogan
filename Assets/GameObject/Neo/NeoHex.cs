using UnityEngine;
using System.Collections;

public static class NeoHex
{
	public static Vector2 Position(HexCoor _coor)
	{
		var _p = _coor.p*NeoConst.HEX_P;
		var _q = _coor.q*NeoConst.HEX_Q;
		return new Vector2(_p + _q.x, _q.y);
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
