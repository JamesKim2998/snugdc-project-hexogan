using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NeoMechanic : MonoBehaviour {
	public int mass = 1;
	public Vector2 com = Vector2.zero;

	public NeoMechanics parent { get; private set; }
	public HexCoor coor { get; private set; }

	#region construct/destruct
	private bool m_IsDestroying = false;

	void Awake()
	{
		EnableRigidbody();
	}

	void OnDestroy()
	{
		m_IsDestroying = true;
		Detach();
	}
	#endregion

	#region parent
	public virtual bool SetParent(NeoMechanics _parent, HexCoor _coor)
	{
		if (parent)
		{
			Debug.LogWarning("Already parent exists! Ignore.");
			return false;
		}

		if (! _parent)
		{
			Debug.LogWarning("Trying to set null parent! Use detach instead. Ignore.");
			return false;
		}

		DisableRigidbody();
		parent = _parent;
		coor = _coor;

		return true;
	}

	public virtual void Detach()
	{
		if (!m_IsDestroying) 
			EnableRigidbody();

		parent = null;
		coor = HexCoor.ZERO;
		// todo: original parent?
		transform.parent = null; 
	}

	#endregion

	#region rigidbody
	private void EnableRigidbody()
	{
		if (rigidbody2D) return;
		gameObject.AddComponent<Rigidbody2D>();
		rigidbody2D.mass = mass;
		rigidbody2D.centerOfMass = com;
		rigidbody2D.drag = 1;
		rigidbody2D.angularDrag = 1;
	}

	private void DisableRigidbody()
	{
		if (!rigidbody2D) return;
		Destroy(gameObject.GetComponent<Rigidbody2D>());
	}
	#endregion
}
