using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public abstract class MaterialDND : DragAndDrop
	{
		protected override void DoMouseDown()
		{
			base.DoMouseDown();
			transform.position += Vector3.back;
			GetComponent<Collider2D>().isTrigger = true;
		}

		protected override void DoMouseDrag()
		{
			base.DoMouseDrag();
			Pivot(false);
		}

		protected override void DoMouseUp()
		{
			base.DoMouseUp();
			Pivot(true);
			Destroy(gameObject);
		}

		protected virtual bool IsLocatable(NeoMechanics _mechanics, NeoBody _body, HexEdge _side)
		{
			return !_body.GetNeighbor(_side);
		}

		protected abstract void Locate(NeoMechanics _mechanics, NeoBody _body, HexEdge _side);
		protected abstract bool Attach(NeoMechanics _mechanics, NeoBody _body, HexEdge _side);

		bool Pivot(bool _attach)
		{
			var _screenPos = Input.mousePosition + (Vector3)offset;
			var _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);

			var _overlaps = Physics2D.OverlapCircleAll(_worldPos, 0.1f, NeoConst.g.mechanicDropMask, -0.1f, 0.1f);

			foreach (var _overlap in _overlaps)
			{
				if (!_overlap) return false;
				var _body = _overlap.GetComponent<NeoBody>();
				if (!_body) return false;
				var _mechanics = _body.parent;
				if (!_mechanics) return false;

				var _bodyCoor = _body.coor;

				var _hexPos = _mechanics.transform.worldToLocalMatrix.MultiplyPoint(transform.position);
				var _side = NeoHex.Side(_hexPos, _bodyCoor);

				if (!IsLocatable(_mechanics, _body, _side))
					continue;

				Locate(_mechanics, _body, _side);

				if (_attach)
					Attach(_mechanics, _body, _side);

				return true;
			}

			return false;
		}
	}
}