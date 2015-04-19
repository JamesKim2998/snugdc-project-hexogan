using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	[RequireComponent(typeof(UIGrid))]
	public class StashGrid : MonoBehaviour
	{
		public UIGrid grid;
		public MaterialCell cellPrf;

		public void Add(NeoMechanicData _item)
		{
			var _cell = cellPrf.Instantiate();
			_cell.SetData(_item);
			_cell.transform.SetParent(grid.transform, false);
		}

		public void Reposition()
		{
			grid.Reposition();
		}
	}

}
