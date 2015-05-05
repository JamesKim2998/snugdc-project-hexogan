#pragma warning disable 0162

using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static partial class DisketteManager
	{
		private const string DAY = "day";
		private const string ASSEMBLY = "assembly";

		private static bool DoLoad(JObject _data)
		{
			do
			{
				var _day = _data[DAY];
				if (_day == null || !DayManager.Load(_day))
					break;

				var _assembly = _data[ASSEMBLY] as JObject;
				if (_assembly == null || !AssemblyManager.Load(_assembly))
					break;

				return true;
			} while (false);

			L.E("DoLoad failed.");
			return true;
		}

		private static JObject DoSave()
		{
			var _data = new JObject();

			do
			{
				var _day = DayManager.Save();
				if (_day == null) break;
				_data[DAY] = _day;

				var _assembly = AssemblyManager.Save();
				if (_assembly == null) break;
				_data[ASSEMBLY] = _assembly;

				return _data;
			} while (false);

			L.E("DoSave failed.");
			return null;
		}
	}
}
