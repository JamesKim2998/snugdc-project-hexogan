using Gem;
using UnityEngine;

namespace ui
{
	public interface IConstructorItem
	{
		string name { get; }
		GameObject MakeItem();
		DragAndDrop MakeDragAndDrop();
	}
} 