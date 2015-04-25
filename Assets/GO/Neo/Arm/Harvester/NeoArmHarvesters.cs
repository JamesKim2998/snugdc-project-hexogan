using System.Collections.Generic;

namespace HX
{
	public class NeoArmHarvesters
	{
		private readonly List<NeoArmHarvester> mElements = new List<NeoArmHarvester>();

		public void Add(NeoArmHarvester _element)
		{
			_element.TurnOn();
			mElements.Add(_element);
		}

		public void Remove(NeoArmHarvester _element)
		{
			mElements.Remove(_element);
			_element.TurnOff();
		}
	}
}