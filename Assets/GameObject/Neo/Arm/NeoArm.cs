using Gem;
using UnityEngine;


namespace HX
{
	public class NeoArm : NeoMechanic
	{
		public NeoArmType type;

		public NeoBody body { get; private set; }
		public HexIdx side { get; private set; }

		public bool Attach(NeoBody _body, HexIdx _side)
		{
			if (body)
			{
				Debug.LogWarning("Body already exists. Ignore.");
				return false;
			}

			if (!_body.parent)
			{
				Debug.LogWarning("Body doesn't have parent. Ignore.");
				return false;
			}

			if (_body.parent)
			{
				if (parent != _body.parent)
				{
					Debug.LogWarning("Parent doesn't match. Ignore.");
					return false;
				}
			}
			else
			{
				SetParent(_body.parent, _body.coor);
			}

			body = _body;
			side = _side;
			transform.parent = _body.transform;
			LocateSide(transform, _side);

			AddCohesion(body);

			return true;
		}

		public static void LocateSide(Transform _transform, HexIdx _idx)
		{
			_transform.localPosition = NeoHex.Side(_idx);
			var _angles = _transform.localEulerAngles;
			_angles.z = _idx.ToDegree();
			_transform.localEulerAngles = _angles;
		}

		public override void Detach()
		{
			if (body) body.RemoveNeighbor(side);
			base.Detach();
		}

	}

}