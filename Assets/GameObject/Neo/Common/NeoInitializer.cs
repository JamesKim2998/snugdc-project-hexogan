using UnityEngine;

namespace HX
{
	public class NeoInitializer : MonoBehaviour
	{
		public LayerMask mechanicDropMask;

		void Awake()
		{
			NeoMechanicDragAndDrop.dropMask = mechanicDropMask;
		}
	}
}