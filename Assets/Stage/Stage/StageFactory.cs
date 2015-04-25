using Gem;
using UnityEngine;

namespace HX
{
	public static class StageFactory
	{
		private const int MAP_TILE_WIDTH = 32;
		private const int MAP_TILE_SIDE = 18;
		private const float MAP_TILE_MARGIN = (MAP_TILE_WIDTH - MAP_TILE_SIDE)/2f;

		public static Vector2 GetPosition(TiledSharp.Map _map, TiledSharp.ObjectGroup.Object _data, uint _mapHeight)
		{
			var _tile = _map.FindTile(_data.Tile.Gid);
			var _tileWidth = _tile.Image.Width.GetValueOrDefault(0);
			var _tileHeight = _tile.Image.Height.GetValueOrDefault(0);

			var x = ((float)_data.X + _tileWidth / 2f - MAP_TILE_WIDTH / 2f) / MAP_TILE_WIDTH;
			var y = (_mapHeight - (float)_data.Y + _tileHeight / 2f - MAP_TILE_WIDTH / 2f) / ((MAP_TILE_SIDE + MAP_TILE_MARGIN) * 2) * HexCoor.SQRT_3;
			
			return new Vector2(x, y);
		}

		public static GameObject Spawn(TiledSharp.Map _map, TiledSharp.ObjectGroup.Object _data)
		{
			var _tile = _map.FindTile(_data.Tile.Gid);

			GOType _type;
			if (!_tile.Properties.TryGetAndParse("type", out _type))
				return null;

			switch (_type)
			{
				case GOType.ASSEMBLY_PIECE:
				{
					NeoMechanicType _mechanicType;
					if (!_tile.Properties.TryGetAndParse("mechanic_type", out _mechanicType))
						return null;

					string _key;
					if (!_data.Properties.TryGet("key", out _key))
						return null;

					var _go = GODB.g.assemblyPiece.Instantiate();
					_go.assembly = AssemblyFactory.MakeWithDic(_mechanicType, _key);
					return _go.gameObject;
				}

				default:
					L.E(L.M.ENUM_UNDEFINED(_type));
					return null;
			}
		}
	}
}