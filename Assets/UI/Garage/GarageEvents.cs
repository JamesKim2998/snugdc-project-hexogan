using System;
using Gem;

namespace HX.UI.Garage
{
	public struct AssembleCommand
	{
		public NeoMechanics mechanics;
		public NeoBody body;
		public HexEdge side;
		public HexCoor coor { get { return body.coor + side; } }
		public Assembly assembly;
	}

	public static class GarageEvents
	{
		public static Action<AssembleCommand> onAssemble;
	}
}