using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public enum TriggerType
	{
		STAGE_EVENT = 1,
	}
}

namespace HX.Stage
{
	public class TriggerStageEvent : Trigger
	{
		private readonly StageEventType mStageEvent;

		public TriggerStageEvent(JObject _def)
			: base(TriggerType.STAGE_EVENT)
		{
			_def.TryGetAndParse("event", out mStageEvent);
		}

		protected override void DoRegister()
		{
			switch (mStageEvent)
			{
				case StageEventType.AFTER_UPDATE:
					StageEvents.onAfterUpdate += DoTrigger;
					break;
				default:
					L.E(L.M.ENUM_UNDEFINED(mStageEvent));
					break;
			}
		}

		protected override void DoUnregister()
		{
			switch (mStageEvent)
			{
				case StageEventType.AFTER_UPDATE:
					StageEvents.onAfterUpdate -= DoTrigger;
					break;
				default:
					L.E(L.M.ENUM_UNDEFINED(mStageEvent));
					break;
			}
		}
	}
}
