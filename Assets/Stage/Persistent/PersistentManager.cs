using System.Collections.Generic;
using Gem;
using Newtonsoft.Json.Linq;

namespace HX.Stage
{
	public struct StageKey
	{
		public readonly AnatomyKey anatomy;
		public readonly OrganKey organ;

		public StageKey(AnatomyKey _anatomy, OrganKey _organ)
		{
			anatomy = _anatomy;
			organ = _organ;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int) anatomy*397) ^ (int) organ;
			}
		}
	}

	public static class PersistentManager
	{
		private static readonly Dictionary<StageKey, PersistentData> sDic = new Dictionary<StageKey, PersistentData>();

		public static bool Load(JObject _data)
		{
			D.Assert(sDic.Empty());

			foreach (var _anatomyKV in _data)
			{
				AnatomyKey _anatomy;
				if (!EnumHelper.TryParse(_anatomyKV.Key, out _anatomy))
					continue;

				foreach (var _organKV in (JObject)_anatomyKV.Value)
				{
					OrganKey _organ;
					if (!EnumHelper.TryParse(_organKV.Key, out _organ))
						continue;

					var _key = new StageKey(_anatomy, _organ);
					sDic[_key] = new PersistentData((JObject)_organKV.Value);
				}
			}

			return true;
		}

		public static JObject Save()
		{
			var _ret = new JObject();

			foreach (var _kv in sDic)
			{
				var _anatomyKey = _kv.Key.anatomy.ToString();
				var _organKey = _kv.Key.organ.ToString();
				
				var _anatomyData = _ret[_anatomyKey] ?? (_ret[_anatomyKey] = new JObject());
				_anatomyData[_organKey] = _kv.Value.Write();
			}

			return _ret;
		}
	}
}