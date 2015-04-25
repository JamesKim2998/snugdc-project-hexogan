using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static class AssemblyFactory
	{
		private static readonly JObject sBodyDic;
		private static readonly JObject sArmDic;

		static AssemblyFactory()
		{
			JToken _dic;
			JsonHelper2.Deserialize(new Path("Resources/Object/assembly_piece.json"), out _dic);
			sBodyDic = (JObject)_dic["bodies"];
			sArmDic = (JObject)_dic["arms"];
		}

		public static Assembly MakeWithDic(NeoMechanicType _type, string _key)
		{
			switch (_type)
			{
				case NeoMechanicType.BODY:
					return MakeBodyWithDic(_key);
				case NeoMechanicType.ARM:
					return MakeArmWithDic(_key);
				default:
					D.Assert(false);
					return null;
			}
		}

		public static BodyAssembly MakeBodyWithDic(string _key)
		{
			JToken _data;
			if (!sBodyDic.TryGet(_key, out _data))
				return null;
			return MakeBody((JObject)_data);
		}

		public static ArmAssembly MakeArmWithDic(string _key)
		{
			JToken _data;
			if (!sArmDic.TryGet(_key, out _data))
				return null;
			return MakeArm((JObject)_data);
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
