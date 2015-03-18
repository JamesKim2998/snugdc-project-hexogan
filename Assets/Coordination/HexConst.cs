using System.Collections.Generic;

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

		public static HexIdx Opposite(this HexIdx i)
		{
			return (HexIdx)(((int)i + 3) % 6);
		}

		public static int ToDegree(this HexIdx i)
		{
			return 60*(int)i;
		}
	}
}