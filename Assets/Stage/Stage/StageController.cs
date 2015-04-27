using Gem;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HX.Stage
{
	public enum StageState
	{
		INIT,
		SETUP,
		START,
		STOP,
		PURGE,
	}

	public partial class StageController : MonoBehaviour
	{
		public static StageController g { get; private set; }

		private StageState mState = StageState.INIT;
		public bool isSetupped { get { return mState != StageState.INIT; } }
		public bool isCompleted { get; private set; }

		private JObject mDef;

		[SerializeField, UsedImplicitly] private WorldController mWorld;
		public WorldController world { get { return mWorld; } }

		private readonly StageObjectiveManager mObjective = new StageObjectiveManager();
		public StageObjectiveManager objective { get { return mObjective; } }

		private readonly StageResult mResult = new StageResult();
		public StageResult result { get { return mResult; } }

		void Start()
		{
			if (TransitionManager.isStageDirty)
			{
				Setup(TransitionManager.stage);
				TransitionManager.MarkStageNotDirty();
			}

			D.Assert(g == null);
			if (g == null)
				g = this;
		}

		void OnDestroy()
		{
			D.Assert(g == this);
			if (g == this)
				g = null;

			Purge();
		}

		void Update()
		{
			TryComplete();
			StageEvents.onAfterUpdate.CheckAndCall();
		}

		public void Setup(StageTransitionData _data)
		{
			if (isSetupped)
			{
				L.E("already setuped.");
				return;
			}

			mState = StageState.SETUP;

			JsonHelper2.Deserialize(_data.defPath, out mDef);

			SetupExpression();
			SetupHarvest();

			world.Setup(_data.tmxPath);

			mObjective.Setup((JObject)mDef["objectives"]);

			Invoke("StartStage", 0);
		}

		private void Purge()
		{
			if (mState == StageState.START)
				StopStage();

			if (mState == StageState.STOP)
				DoPurge();
		}

		void DoPurge() 
		{
			mState = StageState.PURGE;
			PurgeHarvest();
			PurgeExpression();
		}

		private void StartStage()
		{
			if (mState != StageState.SETUP)
			{
				L.E("start when setupped.");
				return;
			}

			mState = StageState.START;
			SpawnNeo();
			objective.Start();
		}

		public void StopStage()
		{
			if (mState != StageState.START)
			{
				L.E("stop when started.");
				return;
			}

			mState = StageState.STOP;
			objective.TryStop();
		}
	}
}