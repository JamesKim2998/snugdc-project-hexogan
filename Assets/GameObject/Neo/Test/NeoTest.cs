using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{
	public class NeoTest : MonoBehaviour
	{
		[Serializable]
		public class BodyData
		{
			public HexCoor coor;
			public NeoBodyType body;
		}

		[Serializable]
		public class ArmData
		{
			public HexCoor bodyCoor;
			public HexIdx bodySide;
			public NeoArmType arm;
		}

		public Neo neo;
		public List<BodyData> editorBodies;
		public List<ArmData> editorArms;

		void Start()
		{
			foreach (var _body in editorBodies)
			{
				var _bodyGO = NeoBodyDatabase.g[_body.body].MakeBody();
				neo.mechanics.Add(_bodyGO, _body.coor);
			}

			foreach (var _arm in editorArms)
			{
				var _armGO = NeoArmDatabase.g[_arm.arm].MakeArm();
				neo.mechanics.Add(_armGO, _arm.bodyCoor, _arm.bodySide);
			}

			neo.mechanics.Build();
		}

	}
}
