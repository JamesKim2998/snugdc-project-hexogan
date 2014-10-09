using UnityEngine;
using System.Collections;

public class NeoArmData : NeoMechanicData, IDatabaseKey<NeoArmType>
{
	public NeoArmType type;
	public NeoArmType Key() { return type; }

	public void Build()
	{
		if (name_ == "")
			name_ = type.ToString();
	}

}
