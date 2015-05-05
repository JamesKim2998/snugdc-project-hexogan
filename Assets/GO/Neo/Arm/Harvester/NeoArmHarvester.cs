using System;
using Gem;
using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public class NeoArmHarvester : MonoBehaviour
	{
		[SerializeField, UsedImplicitly]
		private NeoArm mArm;

		[SerializeField, UsedImplicitly]
		private HarvestField mHarvestField;

		public static Action<HarvestField, Harvestable> onHarvest;

		void Start()
		{
			mHarvestField.onHarvest += OnHarvest;
		}

		public void TurnOn()
		{
			mHarvestField.TurnOn();
		}

		public void TurnOff()
		{
			mHarvestField.TurnOff();
		}

		private void OnHarvest(HarvestField _field, Harvestable _target)
		{
			D.Assert(mArm.parent);
			if (mArm.parent == null) return;

			if (_target.type == HarvestableType.GLUCOSE_PIECE)
				OnHarvest((GlucosePiece)_target);

			onHarvest.CheckAndCall(_field, _target);
		}

		private void OnHarvest(GlucosePiece _target)
		{
			D.Assert(mArm.parent);
			mArm.parent.energyController.Charge(_target.amount);
		}
	}
}