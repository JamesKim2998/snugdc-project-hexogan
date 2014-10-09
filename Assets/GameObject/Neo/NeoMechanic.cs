using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NeoMechanic : MonoBehaviour {
	public int mass = 1;
	[HideInInspector]
	public NeoMechanics mechanics;

	public HexCoor coor
	{
		get { return NeoHex.Coor(transform.localPosition); }
	}

	public NeoMechanic()
	{}

}
