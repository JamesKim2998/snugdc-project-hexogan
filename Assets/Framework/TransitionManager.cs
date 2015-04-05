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
		public static bool isWorldDirty { get; private set; }

		public static WorldTransitionData world { get; private set; }

		public static void StartWorld(WorldTransitionData _data)
		{
			if (Application.isLoadingLevel)
			{
				L.E("transition on going");
				return;
			}

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