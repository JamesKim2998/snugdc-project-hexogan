using Gem;

namespace HX
{
	public class NeoArmData : NeoMechanicData, IDBKey<NeoArmType>
	{
		public NeoArmType key { get; set; }

		public override NeoMechanicType mechanicType
		{
			get { return NeoMechanicType.ARM; }
		}

		public NeoArm MakeArm()
		{
			return mechanicPrf.Instantiate().GetComponent<NeoArm>();
		}

#if UNITY_EDITOR
		public void Build()
		{
			if (name == "")
				name = key.ToString();
		}
#endif

	}
}