using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static partial class DisketteManager
	{
		private const string DEFAULT_SAVE_FILE = "default";

		public static bool isLoaded { get; private set; }
		public static string filename { get; private set; }

		private static FullPath FullPath(string _filename)
		{
			return new FullPath("Resources/Save/" + _filename + ".json");
		}

		public static bool Load(string _filename)
		{
			if (isLoaded)
			{
				L.E("trying to load again.");
				return true;
			}

			JObject _data;
			if (!JsonHelper2.Deserialize(FullPath(_filename), out _data))
				return false;

			if (!DoLoad(_data))
				return false;

			filename = _filename;
			isLoaded = true;

			return true;
		}

		public static void LoadOrDefault(string _filename)
		{
			if (Load(_filename))
				return;

			if (!Load(DEFAULT_SAVE_FILE))
			{
				L.E("load default failed.");
				return;
			}

			filename = _filename;
		}

		public static bool Save()
		{
			if (!isLoaded)
			{
				L.E("save before load.");
				return false;
			}

			var _data = DoSave();

			if (_data == null)
				return false;

			// todo: must backup
			if (!JsonHelper2.Serialize(FullPath(filename), _data))
				return false;

			return true;
		}
	}
}