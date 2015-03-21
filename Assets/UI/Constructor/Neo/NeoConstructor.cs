using Gem;
using UnityEngine;

namespace HX.UI
{
	public class NeoConstructor : MonoBehaviour
	{
		public ConstructorGrid bodyGrid;
		public ConstructorGrid armGrid;

		public MouseActor hammerPrf;

		void Start()
		{
			foreach (var _data in NeoBodyDB.g)
				bodyGrid.Add(new NeoConstructorItem(_data.Value));
			foreach (var _data in NeoArmDB.g)
				armGrid.Add(new NeoConstructorItem(_data.Value));

			bodyGrid.Reposition();
			armGrid.Reposition();
		}

		public void PickHammer()
		{
			var _hammer = hammerPrf.Instantiate();
			_hammer.gameObject.AddComponent<NeoConstructorHammer>();
		}
	}

}