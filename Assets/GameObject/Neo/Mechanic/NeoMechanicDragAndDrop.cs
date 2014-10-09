﻿using System.Net.Mail;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public abstract class NeoMechanicDragAndDrop : DragAndDrop
{
	static public LayerMask dropMask;

	protected override void _OnMouseDown()
	{
		base._OnMouseDown();
		transform.position += Vector3.back;
		collider2D.isTrigger = true;
	}

	protected override void _OnMouseDrag()
	{
		base._OnMouseDrag();
		Pivot();
	}

	protected override void _OnMouseUp()
	{
		base._OnMouseUp();
		Pivot(true);
		transform.position += Vector3.forward;
		collider2D.isTrigger = false;
	}

	protected virtual bool IsLocatable(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, int _side)
	{
		return !_body.GetNeighbor(_side);
	}

	protected abstract void Locate(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, int _side);
	protected abstract bool Attach(NeoMechanics _mechanics, NeoBody _body, HexCoor _coor, int _side);

	bool Pivot(bool _attach = false)
	{
		var _screenPos = Input.mousePosition + (Vector3) offset;
		var _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);

		var _overlaps = Physics2D.OverlapCircleAll(_worldPos, 0.1f, dropMask, -0.1f, 0.1f);

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

			if (!IsLocatable(_mechanics, _body, _bodyCoor, _side))
				continue;

			if (_attach)
			{
				if (Attach(_mechanics, _body, _bodyCoor, _side))
				{
					Destroy(this);
					return true;
				}
			}

			Locate(_mechanics, _body, _bodyCoor, _side);

			return true;
		}

		return false;
	}
}
