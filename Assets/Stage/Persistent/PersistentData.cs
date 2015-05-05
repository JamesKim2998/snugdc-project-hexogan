using System.Collections.Generic;
using Gem;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HX.Stage
{
	public struct LostAssembly
	{
		public readonly Vector2 coor;
		public readonly int lostDay;
		public readonly Assembly assembly;

		public LostAssembly(Vector2 _coor, int _lostDay, Assembly _assembly)
		{
			coor = _coor;
			lostDay = _lostDay;
			assembly = _assembly;
		}

		public LostAssembly(JObject _data)
		{
			UnityHelper.Deserialize((JArray)_data["coor"], out coor);
			lostDay = (int)_data["lostDay"];

			var _assemblyID = (AssemblyID)(int)_data["assemblyID"];
			assembly = AssemblyFactory.Make(_assemblyID, (JObject)_data["assembly"]);
		}

		public JObject Write()
		{
			var _ret = new JObject();
			_ret["coor"] = UnityHelper.Serialize(coor);
			_ret["lostDay"] = lostDay;
			_ret["assemblyID"] = (int)assembly.id;

			var _assembly = new JObject();
			_ret["assembly"] = _assembly;
			assembly.Write(_assembly);

			return _ret;
		}
	}

	public class PersistentData
	{
		public readonly List<LostAssembly> lostAssemblies = new List<LostAssembly>();

		public PersistentData(JObject _data)
		{
			foreach (var _lostAssembly in (JArray)_data["lostAssemblies"])
				lostAssemblies.Add(new LostAssembly((JObject)_lostAssembly));
		}

		public JObject Write()
		{
			var _ret = new JObject();

			var _lostAssemblies = new JArray();
			_ret["lostAssemblies"] = _lostAssemblies;

			foreach (var _lostAssembly in lostAssemblies)
				_lostAssemblies.Add(_lostAssembly.Write());

			return _ret;
		}
	}
}