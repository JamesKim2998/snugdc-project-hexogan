using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
	public Vector2 offset { get; private set; }

	#region physics
	private bool m_Physics = false;
	public bool physics
	{
		get { return m_Physics; }
		set
		{
			if (physics == value) return;
			m_Physics = value;
			if (physics)
				m_Velocity = Vector2.zero;
		}
	}

	public Vector2 m_Velocity;
	#endregion

	void Awake()
	{
		if (rigidbody2D) 
			physics = true;
	}

	void OnMouseDown()
	{
		offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseDrag()
	{
		var _positionOld = transform.position;
		var _position = _positionOld;
		var _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_position.x = _mousePosition.x;
		_position.y = _mousePosition.y;
		transform.position = _position + (Vector3) offset;

		var _velocity = ((Vector2)transform.position - (Vector2)_positionOld) / Time.deltaTime;
		m_Velocity = Vector2.Lerp(m_Velocity, _velocity, 10 * Time.deltaTime);
	}

	void OnMouseUp()
	{
		if (rigidbody2D)
			rigidbody2D.velocity = m_Velocity;
	}
}
