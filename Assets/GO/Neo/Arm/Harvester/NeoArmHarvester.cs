using System;
using Gem;
using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public class NeoArmHarvester : MonoBehaviour
	{
		[SerializeField, UsedImplicitly] private HarvestField mHarvestField;

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
			if (_target.type == HarvestableType.GLUCOSE_PIECE)
			{
				// todo: 즉시 neo의 에너지를 증가
			}

			onHarvest.CheckAndCall(_field, _target);
		}
	}
}