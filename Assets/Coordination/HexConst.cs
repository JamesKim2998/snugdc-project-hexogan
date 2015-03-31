using System.Collections.Generic;
using UnityEngine;

namespace Gem
{
	public enum HexVertex
	{
		TR, T, LT, 
		LB, L, BR, 
	}

	public enum HexEdge
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
		public static readonly float COS_30 = Mathf.Cos(Mathf.PI/6);
		public static readonly float COS_60 = Mathf.Cos(Mathf.PI/3);

		public static HexVertex Next(this HexVertex _this)
		{
			return (_this != (HexVertex)5) ? ++_this : 0;
		}

		public static HexEdge Next(this HexEdge _this)
		{
			return (_this != (HexEdge)5) ? ++_this : 0;
		}

		public static IEnumerable<HexVertex> GetVertexs()
		{
			return EnumHelper.GetValues<HexVertex>();
		}

		public static IEnumerable<HexEdge> GetEdges()
		{
			return EnumHelper.GetValues<HexEdge>();
		}

		public static int ToDeg(this HexVertex v)
		{
			return 60 * (int)v + 30;
		}

		public static int ToDeg(this HexEdge e)
		{
			return 60 * (int)e;
		}

		public static float ToRad(this HexVertex v)
		{
			return v.ToDeg() * Mathf.Deg2Rad;
		}

		public static float ToRad(this HexEdge e)
		{
			return e.ToDeg() * Mathf.Deg2Rad;
		}

		public static HexVertex Opposite(this HexVertex v)
		{
			return (HexVertex)(((int)v + 3) % 6);
		}

		public static HexEdge Opposite(this HexEdge e)
		{
			return (HexEdge)(((int)e + 3) % 6);
		}

		public static HexVertex Vertex(Vector2 _val)
		{
			var _angle = _val.ToRad();
			var _vertex = _angle/(Mathf.PI/3) + 6;
			return (HexVertex)(((int)_vertex) % 6);
		}

		public static HexEdge Edge(Vector2 _val)
		{
			var _angle = _val.ToRad();
			var _edge = _angle/(Mathf.PI/3) + 6.5f;
			return (HexEdge) (((int) _edge)%6);
		}

		public static Vector2 ToVector2(this HexVertex _this)
		{
			return MetricHelper.DegToVector(_this.ToDeg())*(0.5f/COS_30);
		}

		public static Vector2 ToVector2(this HexEdge _this)
		{
			return MetricHelper.DegToVector(_this.ToDeg());
		}
	}
}