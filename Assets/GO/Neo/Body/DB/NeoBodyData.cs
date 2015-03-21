using Gem;

namespace HX
{
	public class NeoBodyData : NeoMechanicData, IDBKey<NeoBodyType>
	{
		public NeoBodyType key { get; set; }

		public NeoBody MakeBody()
		{
			return mechanicPrf.Instantiate().GetComponent<NeoBody>();
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