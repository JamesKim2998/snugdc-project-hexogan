using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class NeoTest : MonoBehaviour
{
	[Serializable]
	public class BodyData
	{
		public HexCoor coor;
		public NeoBody body;
	}

	[Serializable]
	public class ArmData
	{
		public HexCoor bodyCoor;
		public int bodySide;
		public NeoArm arm;
	}

	public Neo neo;
	public List<BodyData> editorBodies;
	public List<ArmData> editorArms;

	void Start () {
		foreach (var _body in editorBodies)
			neo.mechanics.Add(_body.body, _body.coor);

		foreach (var _arm in editorArms)
			neo.mechanics.Add(_arm.arm, _arm.bodyCoor, _arm.bodySide);

		neo.mechanics.Build();
	}
	
}
