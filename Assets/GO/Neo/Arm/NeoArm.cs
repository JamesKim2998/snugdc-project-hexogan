using Gem;
using UnityEngine;

namespace HX
{
	public class NeoArm : NeoMechanic
	{
		public NeoArmType type;

		public NeoBody body { get; private set; }
		public HexEdge side { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			mechanicType = NeoMechanicType.ARM;
		}

		public bool Attach(NeoBody _body, HexEdge _side)
		{
			if (body)
			{
				Debug.LogWarning("body already exists. ignore.");
				return false;
			}

			if (!_body.parent)
			{
				Debug.LogWarning("body doesn't have parent. ignore.");
				return false;
			}

			if (_body.parent)
			{
				if (parent != _body.parent)
				{
					Debug.LogWarning("parent doesn't match. ignore.");
					return false;
				}
			}
			else
			{
				SetParent(_body.parent, _body.coor);
			}

			body = _body;
			side = _side;

			transform.SetParent(_body.transform, false);
			LocateSide(transform, _side);

			AddCohesion(body);

			return true;
		}

		public static void LocateSide(Transform _transform, HexEdge _idx)
		{
			_transform.localPosition = NeoHex.Side(_idx);
			var _angles = _transform.localEulerAngles;
			_angles.z = _idx.ToDeg();
			_transform.localEulerAngles = _angles;
		}

		public override void Detach()
		{
			if (body) body.RemoveNeighbor(side);
			base.Detach();
		}

	}

}