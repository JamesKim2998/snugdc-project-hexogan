using System;

namespace HX
{
	public enum StageEventType
	{
		AFTER_UPDATE = 1,
	}

	public class StageEvents
	{
		public static Action onAfterUpdate;
		public static Action<Assembly> onAcquireAssembly;
	}
}
