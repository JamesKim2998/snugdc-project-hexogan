using System.Collections.Generic;
using System.Linq;
using FullInspector;
using UnityEngine;

namespace HX
{
	public abstract class CellPartData : BaseScriptableObject
	{
		public new abstract string name { get; }
		public Sprite sprite;
		public GameObject goPrf;
		public GameObject constructorItemPrf;
		public List<ScriptableObject> properties;

		public virtual GameObject MakeGO()
		{
			return Instantiate(goPrf);
		}

		public T GetProperty<T>() where T : Object
		{
			return properties.OfType<T>().FirstOrDefault();
		}
	}
}