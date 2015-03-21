#if UNITY_EDITOR

using UnityEngine;

namespace HX
{
	public class EmitterTest : MonoBehaviour
	{
		public Emitter emitter;

		public void Shoot()
		{
			if (emitter.IsShootable())
				emitter.Shoot();
		}
	}
}

#endif