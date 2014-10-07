using System;
using System.Collections;
using UnityEngine;

public static class NeoConst {
	public const float HEX_SIDE = 0.29f;
	public const float HEX_P = 0.5023f;
	public static readonly Vector2 HEX_Q = new Vector2(HEX_P / 2, 1.5f * HEX_SIDE);
}
