using UnityEngine;
using System.Collections;

public class NeoArmData : MonoBehaviour, IDatabaseKey<NeoArmType>
{
	public NeoArmType type;
	public NeoArmType Key() { return type; }
}
