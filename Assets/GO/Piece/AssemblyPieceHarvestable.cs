using Gem;

namespace HX
{
	public class AssemblyPieceHarvestable : Harvestable
	{
		public AssemblyPieceHarvestable()
			: base(HarvestableType.ASSEMBLY_PIECE)
		{}

		protected override void DoBeHarvested()
		{
			L.W("be harvested.");
		}
	}
}
