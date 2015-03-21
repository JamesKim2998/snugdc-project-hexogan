using Gem;
using UnityEngine;

namespace HX
{
	public class EmitterData : ScriptableObject, IDBKey<EmitterType>
	{
		public EmitterType key { get; set; }

		public Emitter emitterPrf;
	}
}