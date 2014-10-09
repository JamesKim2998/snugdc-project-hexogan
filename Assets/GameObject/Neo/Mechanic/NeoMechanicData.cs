using UnityEngine;
using System.Collections;

public class NeoMechanicData : MonoBehaviour {

	public string name_;
	public Sprite sprite;

	public NeoMechanic mechanicPrf;
	public GameObject constructorItemPrf;

	public GameObject MakeConstructorItem()
	{
		GameObject _item;
		if (constructorItemPrf)
		{
			_item = (GameObject)Instantiate(constructorItemPrf);
		}
		else
		{
			_item = new GameObject("item_" + name_);
			var _renderer = _item.AddComponent<UI2DSprite>();
			_renderer.sprite2D = sprite;
			_renderer.MakePixelPerfect();
		}

		return _item;
	}
}
