using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public static class ConditionFactory
	{
		public static Condition Make(JObject _def)
		{
			ConditionType _type;
			if (!_def.TryGetAndParse("type", out _type))
				return null;

			switch (_type)
			{
				case ConditionType.STAGE_REACH:
					return new Stage.ConditionStageReach(_def);
				default:
					L.E(L.M.ENUM_UNDEFINED(_type));
					return null;
			}
		}
	}
}
