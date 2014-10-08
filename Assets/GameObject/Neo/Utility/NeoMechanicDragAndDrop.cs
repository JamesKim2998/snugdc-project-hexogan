using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DragAndDrop))]
public class NeoMechanicDragAndDrop : MonoBehaviour
{
	public LayerMask dropMask;

	void OnMouseDown()
	{
		transform.position += Vector3.back;
		collider2D.isTrigger = true;
	}

	void OnMouseDrag()
	{
		Pivot();
	}

	void OnMouseUp()
	{
		transform.position += Vector3.forward;
		collider2D.isTrigger = false;
	}

	void Pivot()
	{
		var _screenPos = Input.mousePosition + (Vector3)GetComponent<DragAndDrop>().offset;
		var _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);

		var _overlap = Physics2D.OverlapCircle(_worldPos, 0.3f, dropMask, -0.1f, 0.1f);
		if (! _overlap) return;
		var _body = _overlap.GetComponent<NeoBody>();
		if (! _body) return;
		var _mechanics = _body.mechanics;
		if (! _mechanics) return;

		var _localPos = _mechanics.transform.TransformPoint(_worldPos);
		int _side;
		var _localCoor = NeoHex.Coor(_localPos, out _side);
		NeoHex.LocateSide(transform, _side);

		Debug.Log(_overlap.name);
		Debug.Log(_localCoor + " " + _side);
	}
}
