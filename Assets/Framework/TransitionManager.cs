using Gem;
using UnityEngine;

namespace HX 
{
	public struct WorldTransitionData
	{
		public string scene;
		public Path tmxPath;
	}

	public static class TransitionManager
	{
		public const string SCENE_LOBBY = "Lobby";
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

		private static void StartScene(string _scene)
		{
			if (!BeforeStart()) return;
			Application.LoadLevel(_scene);
		}

		public static void StartLobby()
		{
			StartScene(SCENE_LOBBY);
		}

		public static void StartGarage()
		{
			StartScene(SCENE_GARAGE);
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