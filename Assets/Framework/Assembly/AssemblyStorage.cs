using System.Collections.Generic;
using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	using AllOwn = Dictionary<AssemblyID, Assembly>;
	using AllBody = Dictionary<NeoBodyType, List<BodyAssembly>>;
	using AllArm = Dictionary<NeoArmType, List<ArmAssembly>>;

	public class AssemblyStorage
	{
		private readonly AllOwn mDic = new AllOwn();
		private readonly AllBody mBodies = new AllBody();
		private readonly AllArm mArms = new AllArm();

		public bool TryGet(AssemblyID _id, out Assembly _out)
		{
			return mDic.TryGet(_id, out _out);
		}

		public bool TryGet(AssemblyID _id, out BodyAssembly _out)
		{
			return (_out = (BodyAssembly) mDic.GetOrDefault(_id)) != null;
		}

		public bool TryGet(AssemblyID _id, out ArmAssembly _out)
		{
			return (_out = (ArmAssembly)mDic.GetOrDefault(_id)) != null;
		}

		public void Add(BodyAssembly _val)
		{
			mDic[_val] = _val;
			var _list = mBodies.GetOrPut(_val.type);
			_list.Add(_val);
		}

		public void Add(ArmAssembly _val)
		{
			mDic[_val] = _val;
			var _list = mArms.GetOrPut(_val.type);
			_list.Add(_val);
		}

		public List<BodyAssembly> GetBodies(NeoBodyType _type)
		{
			return mBodies.GetOrDefault(_type);
		}

		public List<ArmAssembly> GetArms(NeoArmType _type)
		{
			return mArms.GetOrDefault(_type);
		}

		public static AssemblyStorage Load(JObject _data)
		{
			var _storage = new AssemblyStorage();

			var _bodies = (JObject)_data["bodies"];
			if (_bodies != null)
			{
				foreach (var _body in _bodies)
				{
					var _assembly = MakeBody(_body.Key, (JObject)_body.Value);
					_storage.Add(_assembly);
				}
			}

			var _arms = (JObject)_data["arms"];
			if (_arms != null)
			{
				foreach (var _arm in _arms)
				{
					var _assembly = MakeArm(_arm.Key, (JObject)_arm.Value);
					_storage.Add(_assembly);
				}
			}

			return _storage;
		}
		

		public JObject Save()
		{
			return null;
		}

		private static BodyAssembly MakeBody(string _key, JObject _data)
		{
			var _id = AssemblyHelper.MakeID(_key);

			NeoBodyType _type;
			if (!EnumHelper.TryParse((string)_data["type"], out _type))
				return null;

			var _staticData = NeoBodyDB.g[_type];

			var _assembly = new BodyAssembly(_id, _staticData);
			_assembly.Read(_data);
			return _assembly;
		}

		private static ArmAssembly MakeArm(string _key, JObject _data)
		{
			var _id = AssemblyHelper.MakeID(_key);

			NeoArmType _type;
			if (!EnumHelper.TryParse((string)_data["type"], out _type))
				return null;

			var _staticData = NeoArmDB.g[_type];

			var _assembly = new ArmAssembly(_id, _staticData);
			_assembly.Read(_data);
			return _assembly;
		}
	}
}
