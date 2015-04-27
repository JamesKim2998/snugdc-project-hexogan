using Gem;

namespace HX
{
	public partial class StageController
	{
		public Neo neo { get; private set; }

		private NeoController mNeoController;

		private void SpawnNeo()
		{
			if (neo)
			{
				L.E("neo already exists.");
				return;
			}

			neo = world.SpawnNeo();
			mNeoController = neo.AddComponent<NeoController>();
			mNeoController.neo = neo;
		}
	}
}
