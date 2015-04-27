using Gem;
using UnityEngine;

namespace HX.Stage
{
	public static class StageFactory
	{
		private const int MAP_TILE_WIDTH = 32;
		private const int MAP_TILE_SIDE = 18;
		private const float MAP_TILE_MARGIN = (MAP_TILE_WIDTH - MAP_TILE_SIDE)/2f;

		public static Vector2 GetPosition(TiledSharp.ObjectGroup.Object _data, uint _mapHeight)
		{
			var _tile = _data.TilesetTile;
			var _tileWidth = _tile.Image.Width.GetValueOrDefault(0);
			var _tileHeight = _tile.Image.Height.GetValueOrDefault(0);

			var x = ((float)_data.X + _tileWidth / 2f - MAP_TILE_WIDTH / 2f) / MAP_TILE_WIDTH;
			var y = (_mapHeight - (float)_data.Y + _tileHeight / 2f - MAP_TILE_WIDTH / 2f) / ((MAP_TILE_SIDE + MAP_TILE_MARGIN) * 2) * HexCoor.SQRT_3;
			
			return new Vector2(x, y);
		}

		public static GOType GetType(TiledSharp.ObjectGroup.Object _data)
		{
			GOType _type;
			var _properties = _data.TilesetTile.Properties;
			return _properties.TryGetAndParse("type", out _type) ? _type : default(GOType);
		}

		public static GameObject Spawn(GOType _type, TiledSharp.ObjectGroup.Object _data)
		{
			var _tile = _data.TilesetTile;

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