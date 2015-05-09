using JetBrains.Annotations;
using UnityEngine;

namespace HX.Stage
{
	public partial class HUDController
	{
		[SerializeField, UsedImplicitly]
		private BaseBar mRoughnessBar;

		private void UpdateRoughnessBar()
		{
			var _neo = StageController.g.neo;
			if (_neo != null)
			{
				mRoughnessBar.value = _neo.mechanics.roughness.value;
			}
		}

		private void SetupRoughnessBar()
		{
			NeoRoughnessController.onMaxChanged += OnRoughnessMaxChanged;
		}

		private void PurgeRoughnessBar()
		{
			NeoRoughnessController.onMaxChanged -= OnRoughnessMaxChanged;
		}

		private void OnRoughnessMaxChanged(int _val, int _old)
		{
			mRoughnessBar.max = _val;
		}
	}
}
