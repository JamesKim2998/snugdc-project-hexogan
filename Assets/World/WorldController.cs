using System;
using Gem;
using UnityEngine;

namespace HX
{
	public class WorldController : MonoBehaviour
	{
		[SerializeField] private CellGrid mCellGrid;

		void Start()
		{
			if (TransitionManager.isWorldDirty)
			{
				Setup(TransitionManager.world);
				TransitionManager.MarkWorldNotDirty();
			}
		}

		void Setup(WorldTransitionData _data)
		{
			if (!mCellGrid.empty)
			{
				L.E("grid should be empty.");
				return;
			}

			TiledSharp.Document _doc;

			try
			{
				_doc = new TiledSharp.Document(_data.tmxPath);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return;
			}
			
			var _map = new TiledSharp.Map(_doc);

			var _gridData = new CellGridData(_map);
			foreach (var kv in _gridData.cells)
			{
				var _cell = CellHelper.MakeCell(kv.Value);
				mCellGrid.Add(_cell, kv.Key);
			}
		}
	}
}