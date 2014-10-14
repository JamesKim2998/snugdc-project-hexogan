using UnityEngine;
using System.Collections;

public abstract class CellPartData : MonoBehaviour {
	public abstract string name_ { get; }
	public Sprite sprite;
	public GameObject goPrf;
	public GameObject constructorItemPrf;

	public virtual GameObject MakeGO()
	{
		return (GameObject) Instantiate(goPrf);
	}
}
