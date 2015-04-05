#if UNITY_EDITOR
using UnityEngine;
using Gem;

namespace HX
{
	public class CellGridTest : MonoBehaviour
	{
		public CellGrid grid;
		public string tmx;

		void Start()
		{
			Load();
		}

		public void Load()
		{
			if (!grid.empty)
			{
				L.E("grid should be empty.");
				return;
			}


			var _doc = new TiledSharp.Document(tmx);

			var _map = new TiledSharp.Map(_doc);
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