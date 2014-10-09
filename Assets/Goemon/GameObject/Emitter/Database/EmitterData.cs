using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EmitterData : MonoBehaviour, IDatabaseKey<EmitterType>
{
	public EmitterType type;
	public EmitterType Key() { return type; }

	public Emitter emitterPrf;
	
}

