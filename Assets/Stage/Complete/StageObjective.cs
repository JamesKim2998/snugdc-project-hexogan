using Newtonsoft.Json.Linq;

namespace HX
{
	public class StageObjective
	{
		public Trigger trigger { get; private set; }
		public Condition condition { get; private set; }

		public static StageObjective Make(JObject _def)
		{
			var _trigger = TriggerFactory.Make((JObject)_def["trigger"]);
			if (_trigger == null) return null;

			var _condition = ConditionFactory.Make((JObject) _def["condition"]);
			if (_condition == null) return null;

			return new StageObjective
			{
				trigger = _trigger, 
				condition = _condition,
			};
		}
	}

}