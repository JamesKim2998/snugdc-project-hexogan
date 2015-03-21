using Gem;

namespace HX
{
	public class NeoArmData : NeoMechanicData, IDBKey<NeoArmType>
	{
		public NeoArmType key { get; set; }

		public void Build()
		{
			if (name == "")
				name = key.ToString();
		}

		public NeoArm MakeArm()
		{
			return MakeMechanic().GetComponent<NeoArm>();
		}

	}
}