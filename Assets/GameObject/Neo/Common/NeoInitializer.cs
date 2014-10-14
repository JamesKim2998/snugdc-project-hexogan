using UnityEngine;
using System.Collections;

public class NeoInitializer : MonoBehaviour
{
	public LayerMask mechanicDropMask;
	
	void Awake()
	{
		NeoMechanicDragAndDrop.dropMask = mechanicDropMask;
	}
}
