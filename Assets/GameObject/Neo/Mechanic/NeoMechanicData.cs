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
		var _go = (GameObject)Instantiate(mechanicPrf.gameObject);
		var _mechanic = _go.GetComponent<NeoMechanic>();
		_mechanic.Setup(this);
		return _mechanic;
	}

	public GameObject MakeConstructorItem()
	{
		GameObject _item;
		if (constructorItemPrf)
		{
			_item = (GameObject)Instantiate(constructorItemPrf);
		}
		else
		{
			_item = new GameObject(name_);
			var _renderer = _item.AddComponent<UI2DSprite>();
			_renderer.sprite2D = sprite;
			_renderer.MakePixelPerfect();
		}

		return _item;
	}
}
