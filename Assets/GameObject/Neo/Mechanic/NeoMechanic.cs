﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NeoMechanic : MonoBehaviour {
	public int mass = 1;
	public Vector2 com = Vector2.zero;

	public NeoMechanicData data { get; private set; }

	public HexCoor coor { get; private set; }

	public Animator animator;

	#region construct/destruct
	private bool m_IsDestroying = false;

	void Awake()
	{
		animator = GetComponent<Animator>();
		EnableRigidbody();
		m_DamageDetector = gameObject.AddComponent<DamageDetector>();
		m_DamageDetector.postDamage += Damage;
	}

	void OnDestroy()
	{
		m_IsDestroying = true;
		m_DamageDetector.postDamage -= Damage;
		Detach();
	}

	public void Setup(NeoMechanicData _data)
	{
		if (data)
		{
			Debug.LogError("Trying to setup data again. Ignore.");
			return;
		}

		data = _data;
		cohesionLeft = data.cohesion;
		durabilityLeft = data.durability;
	}
	#endregion

	#region parent
	private NeoMechanics m_Parent;
	public NeoMechanics parent
	{
		get { return m_Parent; }
		private set
		{
			if (parent == value)
				return;
			m_Parent = value;
			if (m_Parent)
			{
				m_DamageDetector.owner = m_Parent.GetInstanceID();
			}
			else
			{
				m_DamageDetector.owner = 0;
				coor = HexCoor.ZERO;
				transform.parent = null;
				cohesionLeft = 0;
			}
		}
	}

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

	#region life
	public float durabilityLeft { get; private set; }
	public float cohesionLeft { get; private set; }
	public DamageDetector m_DamageDetector;

	public void Damage(AttackData _attackData)
	{
		if (cohesionLeft > 0 && parent)
		{
			cohesionLeft -= _attackData;
			if (cohesionLeft <= 0)
				parent.Remove(this);
		}
		else if (durabilityLeft > 0)
		{
			durabilityLeft -= _attackData;
			if (durabilityLeft <= 0)
				Decay();
		}
		else if (durabilityLeft <= 0)
		{
			if (parent) parent.Remove(this);
			Decay();
		}
	}

	public void Decay()
	{
		Destroy(gameObject, 1);
	}
	#endregion
}
