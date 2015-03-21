using Gem;
using UnityEngine;

namespace HX.UI
{
	public class CellConstructor : MonoBehaviour
	{
		public CellGrid cellGrid;
		public Cell cellPrf;

		public ConstructorGrid plasmGrid;
		public ConstructorGrid wallGrid;

		public MouseActor hammerPrf;

		void Start()
		{
			foreach (var _data in CellPlasmDB.g)
				plasmGrid.Add(MakeItem(_data.Value));
			foreach (var _data in CellWallDB.g)
				wallGrid.Add(MakeItem(_data.Value));

			plasmGrid.Reposition();
			wallGrid.Reposition();
		}

		public void PickHammer()
		{
			var _hammer = hammerPrf.Instantiate();
			_hammer.gameObject.AddComponent<CellConstructorHammer>();
		}

		CellConstructorItem MakeItem(CellPartData _data)
		{
			return new CellConstructorItem(_data) {cellGrid = cellGrid, cellPrf = cellPrf};
		}
	}
}
