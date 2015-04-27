using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static class TriggerFactory
	{
		public static Trigger Make(JObject _def)
		{
			TriggerType _type;
			if (!_def.TryGetAndParse("type", out _type))
				return null;

			switch (_type)
			{
				case TriggerType.STAGE_EVENT:
					return new TriggerStageEvent(_def);
				default:
					L.E(L.M.ENUM_UNDEFINED(_type));
					return null;
			}
		}
	}
}
