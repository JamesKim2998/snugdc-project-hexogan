using Gem;

namespace HX
{
	public static class NeoMechanicFactory
	{
		public static NeoBody Create(NeoBodyData _data)
		{
			D.Assert(_data);
			return _data.MakeBody();
		}

		public static NeoArm Create(NeoArmData _data)
		{
			D.Assert(_data);
			return _data.MakeArm();
		}
	}
}