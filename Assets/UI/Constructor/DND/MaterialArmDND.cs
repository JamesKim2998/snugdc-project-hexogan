using Gem;

namespace HX.UI.Garage
{
	public class MaterialArmDND : MaterialDND
	{
		public NeoArmType type;

		protected override void Locate(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			var _posOld = _mechanics.transform.localToWorldMatrix.MultiplyPoint(NeoHex.Position(_body.coor) + NeoHex.Side(_side));
			var _posNew = transform.position;
			_posOld.z = _posNew.z;
			transform.position = _posOld;

			var _angleNew = _body.transform.eulerAngles.z + _side.ToDeg();
			var _angleOld = transform.eulerAngles;
			_angleOld.z = _angleNew;
			transform.eulerAngles = _angleOld;
		}

		protected override bool Attach(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			var _data = NeoArmDB.g[type];
			var _arm = NeoMechanicFactory.Create(_data);
			_mechanics.Add(_arm, _body.coor, _side);
			return true;
		}
	}
}