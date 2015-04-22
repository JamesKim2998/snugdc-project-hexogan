using Gem;

namespace HX
{
	public static class NeoMechanicFactory
	{
		public static NeoBody Create(BodyAssembly _assembly)
		{
			D.Assert(_assembly != null);
			return _assembly.staticData.MakeBody();
		}

		public static NeoArm Create(ArmAssembly _assembly)
		{
			D.Assert(_assembly != null);
			return _assembly.staticData.MakeArm();
		}
	}
}