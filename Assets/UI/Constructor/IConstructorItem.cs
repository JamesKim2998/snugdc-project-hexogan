using Gem;
using UnityEngine;

namespace HX.UI
{
	public interface IConstructorItem
	{
		string name { get; }
		GameObject MakeItem();
		DragAndDrop MakeDragAndDrop();
	}
} 