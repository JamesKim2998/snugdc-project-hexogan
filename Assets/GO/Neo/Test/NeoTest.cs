#if UNITY_EDITOR

using UnityEngine;

namespace HX
{
	public class NeoTest : MonoBehaviour
	{
		[SerializeField] 
		private Neo mNeo;

		[SerializeField]
		private NeoStructure mStructure;

		void Start()
		{
			mNeo.mechanics.Build(mStructure);
		}
	}
}

#endif