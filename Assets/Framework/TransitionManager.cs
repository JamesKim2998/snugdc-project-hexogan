using Gem;
using UnityEngine;

namespace HX 
{
	public struct WorldTransitionData
	{
		public string scene;
		public string tmxPath;
	}

	public static class TransitionManager
	{
		public const string SCENE_GARAGE = "Garage";

		public static bool isWorldDirty { get; private set; }

		public static WorldTransitionData world { get; private set; }

		private static bool BeforeStart()
		{
			if (Application.isLoadingLevel)
			{
				L.E("transition on going");
				return false;
			}

			return true;
		}

		public static void StartGarage()
		{
			if (!BeforeStart())
				return;

			Application.LoadLevel(SCENE_GARAGE);
		}

		public static void StartWorld(WorldTransitionData _data)
		{
			if (!BeforeStart())
				return;

			isWorldDirty = true;
			world = _data;

			D.Assert(!string.IsNullOrEmpty(world.scene));

			Application.LoadLevel(world.scene);
		}

		public static void MarkWorldNotDirty()
		{
			D.Assert(isWorldDirty);
			isWorldDirty = false;
		}
	}
}