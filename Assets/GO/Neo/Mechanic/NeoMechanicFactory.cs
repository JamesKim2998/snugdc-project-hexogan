using Gem;

namespace HX
{
	public static class NeoMechanicFactory
	{
		private static void Init(NeoMechanic _mechanic, Assembly _assembly)
		{
			_mechanic.assemblyID = _assembly;
		}

		public static NeoBody Create(BodyAssembly _assembly)
		{
			D.Assert(_assembly != null);
			var _mechanic = _assembly.staticData.MakeBody();
			Init(_mechanic, _assembly);
			return _mechanic;
		}

		public static NeoArm Create(ArmAssembly _assembly)
		{
			D.Assert(_assembly != null);
			var _mechanic = _assembly.staticData.MakeArm();
			Init(_mechanic, _assembly);
			return _mechanic;
		}
	}
}