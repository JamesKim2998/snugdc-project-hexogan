using System;
using System.IO;
using Gem;
using Newtonsoft.Json.Linq;
using Directory = Gem.Directory;
using Path = Gem.Path;

namespace HX
{
	public static partial class DisketteManager
	{
		private const string DEFAULT_SAVE_FILE = "default";

		private static int sBackupCount = 0;

		public static bool isLoaded { get; private set; }
		public static string filename { get; private set; }

		private static Path GetSavePath(string _filename)
		{
			return new Path("Resources/Save/" + _filename + ".json");
		}

		public static bool Load(string _filename)
		{
			if (isLoaded)
			{
				L.E("trying to load again.");
				return true;
			}

			JObject _data;
			if (!JsonHelper2.Deserialize(GetSavePath(_filename), out _data))
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

			Backup();

			var _path = GetSavePath(filename);
			if (!JsonHelper2.Serialize(_path, _data))
				return false;

			return true;
		}

		private static Directory GetBackupDir()
		{
			return new Directory("Resources/Backup");
		}

		private static Path GetBackupPath()
		{
			var _resolve = filename + "_" + DateTime.Now.ToFileTime() + "_" + ++sBackupCount + ".json";
			return GetBackupDir() / new Path(_resolve);
		}

		private static bool Backup()
		{
			var _path = GetSavePath(filename);
			if (!_path.Exists())
				return false;

			GetBackupDir().CreateIfNotExists();
			File.Move(_path, GetBackupPath());
			return true;
		}
	}
}