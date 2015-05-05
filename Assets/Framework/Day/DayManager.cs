using Newtonsoft.Json.Linq;

namespace HX
{
	public static class DayManager
	{
		public static int day { get; private set; }

		public static void Reset(int _day)
		{
			day = _day;
		}

		public static void Proceed()
		{
			++day;
		}

		public static bool Load(JToken _data)
		{
			if (_data.Type != JTokenType.Integer)
				return false;
			day = (int)_data;
			return true;
		}

		public static JToken Save()
		{
			return new JValue(day);
		}
	}
}