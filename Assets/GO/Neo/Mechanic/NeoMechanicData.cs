using System.Collections.Generic;
using System.Linq;
using FullInspector;
using UnityEngine;

namespace HX
{
	public abstract class NeoMechanicData : BaseScriptableObject
	{
		public abstract NeoMechanicType mechanicType { get; }

		public new string name;

		public NeoMechanic mechanicPrf;

		public int durability = 5;
		public int cohesion = 5;

		public int energyConsumption;

		public GameObject mMaterialPrf;

		// note: it need to be compared explicitly.
		public GameObject materialPrf
		{
			get { return (mMaterialPrf != null) ? mMaterialPrf : mechanicPrf.gameObject; }
		}

		public List<ScriptableObject> properties;

		public T GetProperty<T>() where T : Object
		{
			return properties.OfType<T>().FirstOrDefault();
		}
	}
}