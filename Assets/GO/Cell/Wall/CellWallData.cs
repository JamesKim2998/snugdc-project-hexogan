using Gem;
using UnityEngine;

namespace HX
{
	public class CellWallData : CellPartData, IDBKey<CellWallType>
	{
		public CellWallType key { get; set; }
		public override string name { get { return key.ToString(); } }

		public int hp;

		public override GameObject MakeGO()
		{
			return MakeWall().gameObject;
		}

		public CellWall MakeWall()
		{
			var _go = base.MakeGO();
			var _wall = _go.GetComponent<CellWall>();
			_wall.Setup(this);
			return _wall;
		}
	}
}