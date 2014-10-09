using UnityEngine;
using System.Collections;

public class NeoBodyData : NeoMechanicData, IDatabaseKey<NeoBodyType>
{
	public NeoBodyType type;
	public NeoBodyType Key() { return type; }

	public void Build()
	{
		if (name_ == "")
			name_ = type.ToString();
	}

}
