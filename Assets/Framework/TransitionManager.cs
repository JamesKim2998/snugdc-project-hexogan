using Gem;
using UnityEngine;

namespace HX 
{
	public struct StageTransitionData
	{
		public string scene;
		public Directory dir;
		public string name;

		public Path defPath { get { return dir / new Path(name + ".json"); } }
		public Path tmxPath { get { return dir / new Path(name + ".tmx"); } }
	}

	public static class TransitionManager
	{
		public const string SCENE_LOBBY = "Lobby";
		public const string SCENE_GARAGE = "Garage";
		
		public static bool isStageDirty { get; private set; }
		public static StageTransitionData stage { get; private set; }

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

		public static void StartStage(StageTransitionData _data, bool _shouldProceedDay)
		{
			if (!BeforeStart())
				return;

			isStageDirty = true;
			stage = _data;

			if (_shouldProceedDay)
				DayManager.Proceed();

			D.Assert(!string.IsNullOrEmpty(stage.scene));

			Application.LoadLevel(stage.scene);
		}

		public static void MarkStageNotDirty()
		{
			D.Assert(isStageDirty);
			isStageDirty = false;
		}
	}
}