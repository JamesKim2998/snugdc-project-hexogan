using UnityEngine;
using System.Collections;

public class NeoArm : NeoMechanic
{
	private NeoBody m_Body;
	public int side { get; private set; }

	public void Attach(NeoBody _body, int _side)
	{
		if (m_Body)
		{
			Debug.LogWarning("Body already exists. Replace.");
			return;
		}

		m_Body = _body;
		side = _side;
		transform.parent = _body.transform;
		NeoHex.LocateSide(transform, _side);
	}
}
