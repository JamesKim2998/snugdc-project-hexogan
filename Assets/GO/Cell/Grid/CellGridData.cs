using System.Collections.Generic;
using Gem;
using TiledSharp;

namespace HX
{
	[System.Serializable]
	public class CellGridData
	{
		public readonly Dictionary<HexCoor, CellData> cells = new Dictionary<HexCoor, CellData>();

		public CellGridData(TiledSharp.Map _map)
		{
			foreach (var kv in _map.TraverseWithHexCoorAndTile("plasm"))
			{
				CellPlasmType _type;
				var _props = kv.Second.Properties;
				if (!_props.TryGetAndParse("type", out _type))
					continue;

				cells[kv.First] = new CellData { plasm = _type };
			}

			foreach (var kv in _map.TraverseWithHexCoorAndTile("wall"))
			{
				CellWallType _type;
				var _props = kv.Second.Properties;
				if (!_props.TryGetAndParse("type", out _type))
					continue;

				CellData _data;
				cells.TryGetValue(kv.First, out _data);
				_data.wall = _type;
				cells[kv.First] = _data;
			}
		}
	}
}
