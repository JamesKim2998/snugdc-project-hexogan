#if UNITY_EDITOR

using UnityEngine;

namespace HX
{
	public class CellTest : MonoBehaviour
	{
		public CellGrid grid;
		public string tmx;

		void Start()
		{
			var _map = new TiledSharp.Map(tmx);
			var _gridData = new CellGridData(_map);

			foreach (var kv in _gridData.cells)
			{
				var _cell = CellHelper.MakeCell(kv.Value);
				grid.Add(_cell, kv.Key);
			}
		}
	}
}

#endif