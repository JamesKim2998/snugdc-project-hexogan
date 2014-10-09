﻿using UnityEngine;
using System.Collections;

public class NeoArmDatabase : Database<NeoArmType, NeoArmData>
{
	public static NeoArmDatabase shared;

	public override void Build()
	{
		base.Build();
		foreach (var _data in this)
			_data.Build();
	}
}