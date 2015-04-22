using System;
using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public abstract class MaterialDND : DragAndDrop
	{
		public Assembly assembly;

		public Action<NeoMechanics, NeoBody, HexEdge, Assembly> onAttach;
 
		protected override void Start()
		{
			base.Start();
			camera = GarageController.g.camera.ui;
		}

		protected override void DoMouseDown()
		{
			base.DoMouseDown();
			transform.position += Vector3.back;
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

		private bool Pivot(bool _attach)
		{
			var _globalMousePos = GarageController.g.camera.GetGlobalMousePosition(offset);

			var _overlaps = Physics2D.OverlapCircleAll(_globalMousePos, 0.1f, NeoConst.g.mechanicDropMask, -0.1f, 0.1f);

			foreach (var _overlap in _overlaps)
			{
				var _body = _overlap.GetComponent<NeoBody>();
				if (!_body) continue;
				var _success = Pivot(_body, _attach);
				if (_success) return true;
			}

			return false;
		}

		private bool Pivot(NeoBody _body, bool _attach)
		{
			var _mechanics = _body.parent;
			if (!_mechanics) return false;

			var _bodyCoor = _body.coor;

			var _worldPos = GarageController.g.camera.UIToWorld(transform.position);
			var _hexPos = _mechanics.transform.worldToLocalMatrix.MultiplyPoint(_worldPos);
			var _side = NeoHex.Side(_hexPos, _bodyCoor);

			if (!IsLocatable(_mechanics, _body, _side))
				return false;

			Locate(_mechanics, _body, _side);

			if (_attach)
			{
				var _success = Attach(_mechanics, _body, _side);
				if (_success && (onAttach != null))
					onAttach(_mechanics, _body, _side, assembly);
			}

			return true;
		}
	}
}