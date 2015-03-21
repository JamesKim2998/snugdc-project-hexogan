using Gem;

namespace HX
{
	public class NeoBodyData : NeoMechanicData, IDBKey<NeoBodyType>
	{
		public NeoBodyType key { get; set; }

#if UNITY_EDITOR
		public void Build()
		{
			if (name == "")
				name = key.ToString();
		}
#endif

		public NeoBody MakeBody()
		{
			return MakeMechanic().GetComponent<NeoBody>();
		}
	}
}