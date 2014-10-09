using UnityEngine;
using System.Collections;

public static class NeoMechanicHelper {
	public static void AddDragAndDrop(NeoMechanic _mechanic)
	{
		if (_mechanic.GetComponent<NeoBody>())
			_mechanic.gameObject.AddComponent<NeoBodyDragAndDrop>();
		if (_mechanic.GetComponent<NeoArm>())
			_mechanic.gameObject.AddComponent<NeoArmDragAndDrop>();
	}
}
