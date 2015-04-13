using Gem;

namespace HX
{
	public static class DisketManager
	{
		private const string DEFAULT_SAVE_FILE = "default";

		public static bool isLoaded { get { return saveData != null; } }
		public static string filename { get; private set; }
		public static SaveData saveData { get; private set; }

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

			SaveData _data;
			if (!JsonHelper2.ObjectWithRaw(FullPath(_filename), out _data))
			{
				L.E("load " + _filename + " failed.");
				return false;
			}

			filename = _filename;
			saveData = _data;

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

			return JsonHelper2.SerializeToFile(FullPath(filename), saveData);
		}
	}
}