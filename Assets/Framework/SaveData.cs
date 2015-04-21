using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static partial class DisketteManager
	{
		private const string ASSEMBLY = "assembly";

		private static bool DoLoad(JObject _data)
		{
			do
			{
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
				var _assembly = AssemblyManager.Save();
				if (_assembly == null) break;
				_data[ASSEMBLY] = _data;
				

				return _data;
			} while (false);

			L.E("DoSave failed.");
			return null;
		}
	}
}
