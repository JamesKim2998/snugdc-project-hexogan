using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gem
{
	[Serializable]
	public struct HexCoor : IEquatable<HexCoor>
	{
		public static readonly HexCoor ZERO = new HexCoor(0, 0);
		private static readonly float SQRT_3 = Mathf.Sqrt(3);

		[SerializeField]
		private int mP;
		[SerializeField]
		private int mQ;

		public HexP p { get { return (HexP)mP; } set { mP = (int)value; } }
		public HexQ q { get { return (HexQ)mQ; } set { mQ = (int)value; } }

		public HexCoor(HexP _p, HexQ _q)
		{
			mP = (int)_p;
			mQ = (int)_q;
		}

		public HexCoor(HexEdge i)
		{
			var _p = 0;
			var _q = 0;

			switch (i)
			{
				case HexEdge.R: _p = 1; _q = 0; break;
				case HexEdge.TR: _p = 0; _q = 1; break;
				case HexEdge.TL: _p = -1; _q = 1; break;
				case HexEdge.L: _p = -1; _q = 0; break;
				case HexEdge.BL: _p = 0; _q = -1; break;
				case HexEdge.BR: _p = 1; _q = -1; break;

				default:
					L.E("invalid idx " + i);
					break;
			}

			mP = _p;
			mQ = _q;
		}

		public HexCoor(TiledSharp.Coor _coor, uint _height)
		{
			mQ = (int)(_height - _coor.Y - 1);
			mP = (int)(_coor.X - mQ/2);
		}

		public static HexCoor Round(Vector2 _val)
		{
			var _q = _val.y/(SQRT_3/2f);
			var _p = _val.x - _q/2;
			return new HexCoor((HexP)Mathf.RoundToInt(_p), (HexQ)Mathf.RoundToInt(_q));
		}

		public static IEnumerable<HexCoor> Adjacents()
		{
			foreach (var i in HexHelper.GetEdges())
				yield return i;
		}

		public IEnumerable<HexCoor> Neighbors()
		{
			foreach (var i in HexHelper.GetEdges())
				yield return this + i;
		}

		public static HexEdge Side(Vector2 _coor, HexCoor _center)
		{
			return HexHelper.Edge(_coor - _center);
		}

		public override string ToString()
		{
			return "( " + mP + ", " + mQ + " )";
		}
		
		public TiledSharp.Coor ToTiledCoor(uint _height)
		{
			var x = (uint)(mP + mQ / 2);
			var y = (uint)(_height - mQ - 1);
			return new TiledSharp.Coor(x, y);
		}

		#region equality op
		public bool Compare(HexP _p, HexQ _q)
		{
			return p == _p && q == _q;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
				return false;
			return this == (HexCoor)obj;
		}

		bool IEquatable<HexCoor>.Equals(HexCoor _other)
		{
			return this == _other;
		}

		public static bool operator ==(HexCoor _this, HexCoor _other)
		{
			return _this.p == _other.p
				   && _this.q == _other.q;
		}

		public static bool operator !=(HexCoor _this, HexCoor _other)
		{
			return !(_this == _other);
		}
		#endregion

		#region conversion op

		public static implicit operator HexCoor(HexEdge _val)
		{
			return new HexCoor(_val);
		}

		public static explicit operator HexEdge(HexCoor _this)
		{
			if (_this.Compare(HexP.ONE, HexQ.ZERO))
				return HexEdge.R;
			if (_this.Compare(HexP.ZERO, HexQ.ONE))
				return HexEdge.TR;
			if (_this.Compare(HexP.MINUS_ONE, HexQ.ONE))
				return HexEdge.TL;
			if (_this.Compare(HexP.MINUS_ONE, HexQ.ZERO))
				return HexEdge.L;
			if (_this.Compare(HexP.ZERO, HexQ.MINUS_ONE))
				return HexEdge.BL;
			if (_this.Compare(HexP.ONE, HexQ.MINUS_ONE))
				return HexEdge.BR;

			L.E("invalid coor " + _this);
			return HexEdge.R;
		}

		#endregion

		#region arithmetic op
		public override int GetHashCode()
		{
			return p.GetHashCode() ^ q.GetHashCode();
		}

		public static HexCoor operator +(HexCoor a, HexCoor b)
		{
			return new HexCoor(a.p + (int)b.p, a.q + (int)b.q);
		}

		public static HexCoor operator *(HexCoor _this, int _scalar)
		{
			return new HexCoor((HexP)((int)_this.p * _scalar), (HexQ)((int)_this.q * _scalar));
		}

		public static implicit operator Vector2(HexCoor _this)
		{
			return new Vector2((int)_this.p + (int)_this.q / 2f, (int)_this.q * SQRT_3 / 2f);
		}

		public static Vector2 operator +(HexCoor _this, HexVertex v)
		{
			return _this + v.ToVector2();
		}

		#endregion

	}
}