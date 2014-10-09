using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum NeoArmType
{
	MOTOR = 1,
}

public static class NeoArmEnumerator
{
	public static IEnumerable<NeoArmType> Go()
	{
		yield return NeoArmType.MOTOR;
	}
}