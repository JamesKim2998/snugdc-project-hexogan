#if UNITY_EDITOR

using UnityEngine;

namespace HX.UI
{
	public class DisketteTest : MonoBehaviour
	{
		public string filename = "test";

		void Start()
		{
			if (!DisketteManager.isLoaded)
				DisketteManager.Load(filename);
		}
	}
}

#endif