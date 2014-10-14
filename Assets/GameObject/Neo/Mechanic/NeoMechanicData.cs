using UnityEngine;
using System.Collections;

public class NeoMechanicData : MonoBehaviour {

	public string name_;
	public Sprite sprite;

	public NeoMechanic mechanicPrf;

	public int durability = 5;
	public int cohesion = 5;

	public GameObject constructorItemPrf;

	public NeoMechanic MakeMechanic()
	{
		var _mechanic = ComponentHelper.Instantiate(mechanicPrf);
		_mechanic.Setup(this);
		return _mechanic;
	}

}
