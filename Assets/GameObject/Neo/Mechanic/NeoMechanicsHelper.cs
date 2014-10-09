using UnityEngine;
using System.Collections;

public static class NeoMechanicsHelper {
	public static void Remove(NeoMechanics _mechanics, NeoMechanic _mechanic)
	{
		var _body = _mechanic.GetComponent<NeoBody>();
		if (_body)
		{
			_mechanics.Remove(_body);
			return;
		}

		var _arm = _mechanic.GetComponent<NeoArm>();
		if (_arm)
		{
			_mechanics.Remove(_arm);
			return;
		}
	}
}
