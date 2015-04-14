#if UNITY_EDITOR

using UnityEngine;

namespace HX.UI
{
	public class LobbyTest : MonoBehaviour
	{
		void Start()
		{
			if (!DisketManager.isLoaded)
				DisketManager.LoadOrDefault("test");

			UserManager.neoStructure = DisketManager.saveData.neoStructure;
		}
	}
}

#endif