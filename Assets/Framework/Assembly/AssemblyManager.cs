using System;
using Gem;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HX
{
	public static class AssemblyManager
	{
		private const string ALLOC_ID = "allocID";
		private const string STORAGE = "storage";
		private const string BLUEPRINT = "blueprint";

		public static AssemblyStorage storage { get; private set; }
		public static NeoBlueprint blueprint { get; private set; }

		static AssemblyManager()
		{
			storage = new AssemblyStorage();
			blueprint = new NeoBlueprint();
		}

		public static bool TryAdd(HexCoor _coor, BodyAssembly _mechanic)
		{
			return blueprint.TryAdd(_coor, _mechanic);
		}

		public static bool Load(JObject _data)
		{
			try
			{
				Assembly.sAllocID = (AssemblyID)(int)_data[ALLOC_ID];

				var _dataStorage = _data[STORAGE] as JObject;
				if (_dataStorage != null)
					storage = AssemblyStorage.Load(_dataStorage);
				else
					storage = new AssemblyStorage();

				var _dataBlueprint = _data[BLUEPRINT] as JObject;
				if (_dataBlueprint != null)
					blueprint = NeoBlueprint.Load(storage, _dataBlueprint);
				else
					blueprint = new NeoBlueprint();

				return true;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return false;
			}
		}

		public static JObject Save()
		{
			var _data = new JObject();

			try
			{
				_data[ALLOC_ID] = (int)Assembly.sAllocID;
				_data[STORAGE] = storage.Save();
				_data[BLUEPRINT] = blueprint.Save();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				_data = null;
			}

			return _data;
		}
	}
}