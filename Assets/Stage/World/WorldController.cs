using System;
using System.Collections.Generic;
using Gem;
using JetBrains.Annotations;
using TiledSharp;
using UnityEngine;

namespace HX
{
	public class WorldMarker
	{
		public readonly string key;
		public readonly Vector2 position;

		public WorldMarker(string _key, Vector2 _position)
		{
			key = _key;
			position = _position;
		}
	}

	public class WorldController : MonoBehaviour
	{
		private bool mIsSetuped;

		private HexCoor mStartPosition;

		[SerializeField, UsedImplicitly]
		private Transform mWorldRoot;
		[SerializeField, UsedImplicitly]
		private CellGrid mCellGrid;

		private readonly Dictionary<string, WorldMarker> mMarkers = new Dictionary<string, WorldMarker>();

		public Neo SpawnNeo()
		{
			var _neo = AssemblyManager.blueprint.Instantiate();
			_neo.transform.SetParent(mWorldRoot, false);
			_neo.transform.position = (Vector2)mStartPosition;
			return _neo;
		}

		public void Setup(Path _tmxPath)
		{
			if (mIsSetuped)
			{
				L.E("already setuped.");
				return;
			}

			mIsSetuped = true;

			TiledSharp.Document _doc;

			try
			{
				_doc = new TiledSharp.Document(_tmxPath);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return;
			}

			var _map = new TiledSharp.Map(_doc);

			SetupMarkers(_map);
			SetupCellGrid(_map);

			var _hexMapHeight = (uint)Mathf.RoundToInt(_map.HexMapHeight);
			foreach (var _datas in _map.ObjectGroups)
				SetupObjects(_datas, _hexMapHeight);
		}

		private void SetupMarkers(TiledSharp.Map _map)
		{
			foreach (var kv in _map.TraverseWithCoorAndTile("marker"))
			{
				WorldMarkerType _markerType;
				var _tile = kv.Second;
				if (!_tile.Properties.TryGetAndParse("marker_type", out _markerType))
					continue;

				var _coor = kv.First;

				switch (_markerType)
				{
					case WorldMarkerType.START_POSITION:
						mStartPosition = new WorldMarkerStartPosition(_coor, _tile);
						break;
					default:
						L.W(L.M.ENUM_UNDEFINED(_markerType));
						return;
				}
			}
		}

		private void SetupCellGrid(Map _map)
		{
			var _gridData = new CellGridData(_map);
			foreach (var kv in _gridData.cells)
			{
				var _cell = CellHelper.MakeCell(kv.Value);
				mCellGrid.Add(_cell, kv.Key);
			}
		}

		private void SetupObjects(ObjectGroup _datas, uint _mapHeight)
		{
			foreach (var _data in _datas.Objects)
				SpawnObject(_data, _mapHeight);
		}

		private void SpawnObject(TiledSharp.ObjectGroup.Object _data, uint _mapHeight)
		{
			var _type = StageFactory.GetType(_data);
			var _position = (Vector2)StageFactory.GetPosition(_data, _mapHeight);

			switch (_type)
			{
				case GOType.WORLD_MARKER:
					AddMarker(_data, _position);
					return;
			}

			var _obj = StageFactory.Spawn(_type, _data);
			if (_obj == null)
			{
				L.W("fail to spawn " + _data.Name);
				return;
			}

			_obj.transform.SetParent(mWorldRoot, false);
			_obj.transform.localPosition = _position;

		}

		public void AddMarker(TiledSharp.ObjectGroup.Object _data, Vector2 _position)
		{
			string _key;
			if (!_data.Properties.TryGet("key", out _key)) return;
			mMarkers.Add(_key, new WorldMarker(_key, _position));
		}

		public WorldMarker FindMarker(string _key)
		{
			WorldMarker _marker;
			mMarkers.TryGet(_key, out _marker);
			return _marker;
		}
	}
}