using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class NeoBodyDragAndDrop : NeoMechanicDragAndDrop
{
	protected override bool IsLocatable(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, int _side)
	{
		if (! base.IsLocatable(_mechanics, _body, _coor, _side))
			return false;

		var _coorSide = HexCoor.FromAdjacent(_side);
		var _locateCoor = _coor + _coorSide;

		for (var _neighbor = 0; _neighbor != 6; ++_neighbor) 
		{
			if (_neighbor == HexCoor.OppositeSide(_side)) 
				continue;

			var _neighborCell = _mechanics.GetBody(_locateCoor + HexCoor.FromAdjacent(_neighbor));
			if (_neighborCell == null) continue;

			if (_neighborCell.data.GetNeighbor(HexCoor.OppositeSide(_neighbor)) != null)
				return false;
		}

		return true;
	}

	protected override void Locate(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, int _side)
	{
		var _posNew = _mechanics.transform.localToWorldMatrix.MultiplyPoint(NeoHex.Position(_coor) + NeoHex.Side(_side) * 2);
		var _posOld = transform.position;
		_posNew.z = _posOld.z;
		transform.position = _posNew;

		var _angleNew = _body.transform.eulerAngles.z;
		var _angleOld = transform.eulerAngles;
		_angleOld.z = _angleNew;
		transform.eulerAngles = _angleOld;
	}

	protected override bool Attach(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, int _side)
	{
		_mechanics.Add(GetComponent<NeoBody>(), _coor + HexCoor.FromAdjacent(_side));
		return true;
	}
}
