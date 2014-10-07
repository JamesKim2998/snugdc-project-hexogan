using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {
	Vector3 m_Offset;

	void OnMouseDown()
	{
		m_Offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseDrag()
	{
		var _curScreenSpace = Input.mousePosition;
		transform.position = Camera.main.ScreenToWorldPoint(_curScreenSpace) + m_Offset;
	}
}
