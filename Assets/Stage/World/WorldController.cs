﻿using System;
using Gem;
using TiledSharp;
using UnityEngine;

namespace HX
{
	public class WorldController : MonoBehaviour
	{
		private bool mIsSetuped;

		private HexCoor mStartPosition;

		private Neo mNeo;
		private NeoController mNeoController;

		[SerializeField] private Transform mWorldRoot;
		[SerializeField] private CellGrid mCellGrid;

		void Start()
		{
			if (TransitionManager.isWorldDirty)
			{
				Setup(TransitionManager.world);
				TransitionManager.MarkWorldNotDirty();
			}
		}

		private void Setup(WorldTransitionData _data)
		{
			if (mIsSetuped)
			{
				L.E("already setuped.");
				return;
			}

			mIsSetuped = true;
			SetupGrid(_data.tmxPath);
		}

		public void SetupGrid(Path _tmxPath)
		{
			if (!mCellGrid.empty)
			{
				L.E("grid should be empty.");
				return;
			}

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
		}

		private void SetupMarkers(Map _map)
		{
			foreach (var kv in _map.TraverseWithCoorAndTile("marker"))
			{
				MarkerType _markerType;
				var _tile = kv.Second;
				if (!_tile.Properties.TryGetAndParse("type", out _markerType))
					continue;

				var _coor = kv.First;

				switch (_markerType)
				{
					case MarkerType.START_POSITION:
						mStartPosition = new LevelMarkerStartPosition(_coor, _tile);
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


		public void StartGame()
		{
			mNeo = AssemblyManager.blueprint.Instantiate();
			mNeo.transform.SetParent(mWorldRoot, false);
			mNeo.transform.position = (Vector2)mStartPosition;
			mNeoController = mNeo.AddComponent<NeoController>();
			mNeoController.neo = mNeo;
		}
	}
}