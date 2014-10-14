using UnityEngine;
using System.Collections;

namespace ui
{
	[RequireComponent(typeof(UIGrid))]
	public class ConstructorGrid : MonoBehaviour
	{
		public UIGrid grid;
		public ConstructorEntityView viewPrf;

		public void Add(IConstructorItem _item)
		{
			var _view = ComponentHelper.Instantiate(viewPrf);
			var _ctrl = _view.gameObject.AddComponent<ConstructorEntityController>();
			_ctrl.SetView(_view);
			_ctrl.SetItem(_item);
			TransformHelper.SetParentWithoutScale(_ctrl, grid);
		}

		public void Reposition()
		{
			grid.Reposition();
		}
	}

}
