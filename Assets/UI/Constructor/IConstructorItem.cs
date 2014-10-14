using UnityEngine;
using System.Collections;

namespace ui
{
	public interface IConstructorItem
	{
		string name { get; }
		GameObject MakeItem();
		DragAndDrop MakeDragAndDrop();
	}
} 