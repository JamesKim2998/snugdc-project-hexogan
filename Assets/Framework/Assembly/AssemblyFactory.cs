using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static class AssemblyFactory
	{
		public static Assembly Make(JObject _data)
		{
			NeoMechanicType _type;
			if (!_data.TryGetAndParse("type", out _type))
				return null;

			switch (_type)
			{
				case NeoMechanicType.BODY:
					return MakeBody(_data);
				case NeoMechanicType.ARM:
					return MakeArm(_data);
				default:
					D.Assert(false);
					return null;
			}
		}

		public static BodyAssembly MakeBody(JObject _data)
		{
			return MakeBody(Assembly.AllocateID(), _data);
		}

		public static BodyAssembly MakeBody(AssemblyID _id, JObject _data)
		{
			NeoBodyType _type;
			if (!_data.TryGetAndParse("type", out _type))
				return null;

			var _staticData = NeoBodyDB.g[_type];

			var _assembly = new BodyAssembly(_id, _staticData);
			_assembly.Read(_data);
			return _assembly;
		}

		public static ArmAssembly MakeArm(JObject _data)
		{
			return MakeArm(Assembly.AllocateID(), _data);
		}

		public static ArmAssembly MakeArm(AssemblyID _id, JObject _data)
		{
			NeoArmType _type;
			if (!_data.TryGetAndParse("type", out _type))
				return null;

			var _staticData = NeoArmDB.g[_type];

			var _assembly = new ArmAssembly(_id, _staticData);
			_assembly.Read(_data);
			return _assembly;
		}
	}
}
