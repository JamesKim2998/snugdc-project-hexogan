﻿using System;

namespace HX.Stage
{
	public enum StageEventType
	{
		AFTER_UPDATE = 1,
	}

	public class StageEvents
	{
		public static Action onAfterUpdate;

		public static Action<NeoMechanic> onMechanicLost;
		public static Action<NeoMechanic> onMechanicDecay;

		public static Action<Assembly> onAcquireAssembly;
	}
}
