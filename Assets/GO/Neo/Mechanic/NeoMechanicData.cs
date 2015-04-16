using System.Collections.Generic;
using System.Linq;
using FullInspector;
using UnityEngine;

namespace HX
{
	public class NeoMechanicData : BaseScriptableObject
	{
		public new string name;

		public NeoMechanic mechanicPrf;

		public int durability = 5;
		public int cohesion = 5;

		public GameObject materialPrf;

		public List<ScriptableObject> properties;

		public T GetProperty<T>() where T : Object
		{
			return properties.OfType<T>().FirstOrDefault();
		}
	}
}