using System.Collections.Generic;
using System.Linq;
using FullInspector;
using Gem;
using UnityEngine;

namespace HX
{
	public class NeoMechanicData : BaseScriptableObject
	{
		public new string name;
		public Sprite sprite;

		public NeoMechanic mechanicPrf;

		public int durability = 5;
		public int cohesion = 5;

		public GameObject constructorItemPrf;

		public List<ScriptableObject> properties;

		public NeoMechanic MakeMechanic()
		{
			var _mechanic = mechanicPrf.Instantiate();
			_mechanic.Setup(this);
			return _mechanic;
		}

		public T GetProperty<T>() where T : Object
		{
			return properties.OfType<T>().FirstOrDefault();
		}
	}
}