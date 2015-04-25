using Gem;

namespace HX
{
	public partial class StageController
	{
		void SetupHarvest()
		{
			NeoArmHarvester.onHarvest += OnHarvest;
		}

		void PurgeHarvest()
		{
			NeoArmHarvester.onHarvest -= OnHarvest;
		}

		private void OnHarvest(HarvestField _field, Harvestable _harvestable)
		{
			switch (_harvestable.type)
			{
				case HarvestableType.ASSEMBLY_PIECE:
					result.Acquire((AssemblyPiece)_harvestable);
					break;
				default:
					L.E(L.M.ENUM_UNDEFINED(_harvestable.type));
					break;
			}
		}
	}
}
