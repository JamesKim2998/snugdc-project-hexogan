using System.Collections.Generic;
using UnityEngine;

namespace Gem
{
	public enum HexIdx
	{
		R, TR, TL, 
		L, BL, BR,
	}

	public enum HexP
	{
		ZERO = 0,
		ONE = 1,
		MINUS_ONE = -1,
	}

	public enum HexQ
	{
		ZERO = 0,
		ONE = 1,
		MINUS_ONE = -1,
	}

	public static class HexHelper
	{
		public static IEnumerable<HexIdx> GetIdxes()
		{
			return EnumHelper.GetValues<HexIdx>();
		}

		public static int ToDegree(this HexIdx i)
		{
			return 60 * (int)i;
		}

		public static HexIdx Opposite(this HexIdx i)
		{
			return (HexIdx)(((int)i + 3) % 6);
		}

		public static HexIdx Side(Vector2 _val)
		{
			var _angle = Mathf.Atan2(_val.y, _val.x);
			var _side = _angle/(Mathf.PI/3) + 6.5f;
			return (HexIdx) (((int) _side)%6);
		}
	}
}