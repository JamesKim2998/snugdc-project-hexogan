using JetBrains.Annotations;
using UnityEngine;

namespace HX.Stage
{
	public partial class HUDController
	{
		[SerializeField, UsedImplicitly]
		private BaseBar mEnergyBar;

		private void UpdateEnergyBar()
		{
			var _neo = StageController.g.neo;
			if (_neo != null)
			{
				mEnergyBar.value = _neo.mechanics.energy.available;
			}
		}

		private void SetupEnergyBar()
		{
			NeoEnergyController.onCapacityChanged += OnEnergyCapacityChanged;
		}

		private void PurgeEnergyBar()
		{
			NeoEnergyController.onCapacityChanged -= OnEnergyCapacityChanged;
		}

		private void OnEnergyCapacityChanged(int _val, int _old)
		{
			mEnergyBar.max = _val;
		}
	}
}
