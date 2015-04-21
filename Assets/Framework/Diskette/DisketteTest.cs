#if UNITY_EDITOR

using UnityEngine;

namespace HX.UI
{
	public class DisketteTest : MonoBehaviour
	{
		void Start()
		{
			if (!DisketteManager.isLoaded)
				DisketteManager.LoadOrDefault("test");
		}
	}
}

#endif