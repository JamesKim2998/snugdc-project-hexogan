using Gem;
using UnityEngine;
using System.Collections;

namespace ui
{
	public class NeoConstructor : MonoBehaviour
	{
		public ConstructorGrid bodyGrid;
		public ConstructorGrid armGrid;

		public MouseActor hammerPrf;

		void Start()
		{
			foreach (var _data in NeoBodyDatabase.shared)
				bodyGrid.Add(new NeoConstructorItem(_data));
			foreach (var _data in NeoArmDatabase.shared)
				armGrid.Add(new NeoConstructorItem(_data));

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