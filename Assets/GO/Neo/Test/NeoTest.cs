#if UNITY_EDITOR

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

		[SerializeField]
		private List<BodyData> mBodies;
		[SerializeField]
		public List<ArmData> mArms;

		void Start()
		{
			foreach (var _body in mBodies)
			{
				var _bodyGO = NeoBodyDB.g[_body.body].MakeBody();
				neo.mechanics.Add(_bodyGO, _body.coor);
			}

			foreach (var _arm in mArms)
			{
				var _armGO = NeoArmDB.g[_arm.arm].MakeArm();
				neo.mechanics.Add(_armGO, _arm.bodyCoor, _arm.bodySide);
			}

			neo.mechanics.Build();
		}

	}
}

#endif