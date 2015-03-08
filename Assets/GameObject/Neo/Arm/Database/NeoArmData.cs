namespace HX
{
	public class NeoArmData : NeoMechanicData, IDatabaseKey<NeoArmType>
	{
		public NeoArmType type;
		public NeoArmType Key() { return type; }

		public void Build()
		{
			if (name_ == "")
				name_ = type.ToString();
		}

		public NeoArm MakeArm()
		{
			return MakeMechanic().GetComponent<NeoArm>();
		}

	}
}