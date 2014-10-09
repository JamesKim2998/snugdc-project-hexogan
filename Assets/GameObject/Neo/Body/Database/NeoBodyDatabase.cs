using UnityEngine;
using System.Collections;

public class NeoBodyDatabase : Database<NeoBodyType, NeoBodyData>
{
	public static NeoBodyDatabase shared;

	public override void Build()
	{
		base.Build();
		foreach (var _data in this)
			_data.Build();
	}
}