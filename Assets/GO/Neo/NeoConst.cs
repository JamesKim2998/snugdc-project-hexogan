using UnityEngine;

namespace HX
{
	public class NeoConst : ScriptableObject
	{
		public const float HEX_SIDE = 0.29f;
		public const float HEX_P = 0.5023f;
		public static readonly Vector2 HEX_Q = new Vector2(HEX_P / 2, 1.5f * HEX_SIDE);

		public static NeoConst g;
		public LayerMask mechanicDropMask;
	}
}