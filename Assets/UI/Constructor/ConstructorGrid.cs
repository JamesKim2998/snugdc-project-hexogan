using Gem;
using UnityEngine;

namespace HX.UI
{
	[RequireComponent(typeof(UIGrid))]
	public class ConstructorGrid : MonoBehaviour
	{
		public UIGrid grid;
		public ConstructorEntityView viewPrf;

		public void Add(IConstructorItem _item)
		{
			var _view = viewPrf.Instantiate();
			var _ctrl = _view.gameObject.AddComponent<ConstructorEntityController>();
			_ctrl.SetView(_view);
			_ctrl.SetItem(_item);
			_ctrl.transform.SetParentIdentity(grid.transform);
		}

		public void Reposition()
		{
			grid.Reposition();
		}
	}

}
