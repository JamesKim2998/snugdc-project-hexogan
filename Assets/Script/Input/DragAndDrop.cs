using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {
	public Vector2 offset { get; private set; }

	void OnMouseDown()
	{
		offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseDrag()
	{
		var _position = transform.position;
		var _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_position.x = _mousePosition.x;
		_position.y = _mousePosition.y;
		transform.position = _position + (Vector3) offset;
	}
}
