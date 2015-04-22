using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class MaterialArmDND : MaterialDND
	{
		protected override void Locate(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			var _posGlobal = _mechanics.transform.localToWorldMatrix.MultiplyPoint(NeoHex.Position(_body.coor) + NeoHex.Side(_side));
			var _posNew = GarageController.g.camera.WorldToUI(_posGlobal);
			transform.SetPos((Vector2)_posNew);
			transform.SetEulerZ(_body.transform.eulerAngles.z + _side.ToDeg());
		}

		protected override bool Attach(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			var _assembly = assembly as ArmAssembly;
			D.Assert(_assembly != null);
			var _arm = NeoMechanicFactory.Create(_assembly);
			return _mechanics.Add(_arm, _body.coor, _side);
		}
	}
}