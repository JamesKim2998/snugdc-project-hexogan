using Gem;
using UnityEngine;
using System.Collections;

namespace HX.UI
{
	public class NeoConstructor : MonoBehaviour
	{
		public ConstructorGrid bodyGrid;
		public ConstructorGrid armGrid;

		public MouseActor hammerPrf;

		void Start()
		{
			foreach (var _data in NeoBodyDatabase.g)
				bodyGrid.Add(new NeoConstructorItem(_data));
			foreach (var _data in NeoArmDatabase.g)
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