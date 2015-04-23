using System.Collections.Generic;
using System.Linq;
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

		public List<BodyAssembly> Get(NeoBodyType _type)
		{
			return mBodies.GetOrPut(_type);
		}

		public List<ArmAssembly> Get(NeoArmType _type)
		{
			return mArms.GetOrPut(_type);
		}

		public IEnumerable<BodyAssembly> GetAvailable(NeoBodyType _type)
		{
			var _assemblies = Get(_type);
			return _assemblies.Where(_obj => _obj.availiable);
		}

		public IEnumerable<ArmAssembly> GetAvailable(NeoArmType _type)
		{
			var _assemblies = Get(_type);
			return _assemblies.Where(_obj => _obj.availiable);
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
			var _ret = new JObject();
			var _saveBodies = new JObject();
			_ret["bodies"] = _saveBodies;
			var _saveArms = new JObject();
			_ret["arms"] = _saveArms;

			foreach (var _bodies in mBodies)
			{
				foreach (var _body in _bodies.Value)
				{
					var _idStr = ((int) _body.id).ToString();
					var _data = new JObject();
					_body.Write(_data);
					_saveBodies[_idStr] = _data;
				}
			}

			foreach (var _arms in mArms)
			{
				foreach (var _arm in _arms.Value)
				{
					var _idStr = ((int)_arm.id).ToString();
					var _data = new JObject();
					_arm.Write(_data);
					_saveArms[_idStr] = _data;
				}
			}

			return _ret;
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
