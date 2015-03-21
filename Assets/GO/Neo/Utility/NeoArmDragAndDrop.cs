using Gem;

namespace HX
{
	public class NeoArmDragAndDrop : NeoMechanicDragAndDrop
	{
		protected override void Locate(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, HexIdx _side)
		{
			var _posOld = _mechanics.transform.localToWorldMatrix.MultiplyPoint(NeoHex.Position(_coor) + NeoHex.Side(_side));
			var _posNew = transform.position;
			_posOld.z = _posNew.z;
			transform.position = _posOld;

			var _angleNew = _body.transform.eulerAngles.z + _side.ToDegree();
			var _angleOld = transform.eulerAngles;
			_angleOld.z = _angleNew;
			transform.eulerAngles = _angleOld;
		}

		protected override bool Attach(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, HexIdx _side)
		{
			_mechanics.Add(GetComponent<NeoArm>(), _coor, _side);
			return true;
		}
	}
}