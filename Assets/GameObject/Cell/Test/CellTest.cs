using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{
	public class CellTest : MonoBehaviour
	{

		[System.Serializable]
		public struct CellData
		{
			public HexCoor coor;
			public CellPlasmType plasm;
			public CellWallType wall;
		}

		public CellGrid grid;

		public List<CellData> cellDatas;

		void Start()
		{
			foreach (var _cellData in cellDatas)
			{
				var _cell = CellHelper.MakeCell(_cellData.plasm, _cellData.wall);
				grid.Add(_cell, _cellData.coor);
			}
		}
	}
}