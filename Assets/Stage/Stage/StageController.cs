using Gem;
using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public partial class StageController : MonoBehaviour
	{
		public static StageController g { get; private set; }

		[SerializeField, UsedImplicitly] private WorldController mWorld;
		public WorldController world { get { return mWorld; } }

		private readonly StageResult mResult = new StageResult();
		public StageResult result { get { return mResult; } }

		void Start()
		{
			Setup();

			D.Assert(g == null);
			if (g == null)
				g = this;
		}

		void OnDestroy()
		{
			D.Assert(g == this);
			if (g == this)
				g = null;
		}

		void Setup()
		{
			SetupHarvest();
		}

		void Purge()
		{
			PurgeHarvest();
		}
	}
}