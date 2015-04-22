using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class MaterialBodyDND : MaterialDND
	{
		protected override bool IsLocatable(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			if (!base.IsLocatable(_mechanics, _body, _side))
				return false;

			var _coorSide = (HexCoor)_side;
			var _locateCoor = _body.coor + _coorSide;

			foreach (var i in HexHelper.GetEdges())
			{
				if (i == _side.Opposite())
					continue;

				var _neighborCell = _mechanics.GetBody(_locateCoor + i);
				if (_neighborCell == null) continue;

				if (_neighborCell.data.GetNeighbor(i.Opposite()) != null)
					return false;
			}

			return true;
		}

		protected override void Locate(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			var _posGlobal = _mechanics.transform.localToWorldMatrix.MultiplyPoint(NeoHex.Position(_body.coor) + NeoHex.Side(_side) * 2);
			var _posNew = GarageController.g.camera.WorldToUI(_posGlobal);
			transform.SetPos((Vector2)_posNew);
			transform.SetEulerZ(_body.transform.eulerAngles.z);
		}

		protected override bool DoAttach(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			var _bodyAssembly = assembly as BodyAssembly;
			var _newBody = NeoMechanicFactory.Create(_bodyAssembly);
			var _ret = _mechanics.Add(_newBody, _body.coor + _side);
			if (!_ret) Destroy(_newBody.gameObject);
			return _ret;
		}
	}
}