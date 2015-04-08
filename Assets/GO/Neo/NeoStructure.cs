using System;
using System.Collections.Generic;
using Gem;

namespace HX
{
	[Serializable]
	public class NeoStructure
	{
		[Serializable]
		public struct BodyData
		{
			public NeoBodyType type;
			public HexCoor coor;
		}

		[Serializable]
		public struct ArmData
		{
			public NeoArmType type;
			public HexCoor coor;
			public HexEdge side;
		}

		public List<BodyData> bodies;
		public List<ArmData> arms;
	}
}