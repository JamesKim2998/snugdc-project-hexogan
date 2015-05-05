using Gem;
using UnityEngine;

namespace HX.Stage
{
	public partial class HUDController : MonoBehaviour
	{
		public static HUDController g { get; private set; }

		void Start()
		{
			SetupEnergyBar();

			D.Assert(g == null);
			if (g == null)
				g = this;
		}

		void OnDestroy()
		{
			PurgeEnergyBar();

			D.Assert(g == this);
			if (g == this)
				g = null;
		}

		void Update()
		{
			UpdateEnergyBar();
		}
	}
}