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

		public HexP p;
		public HexQ q;

		public HexCoor(HexP _p, HexQ _q)
		{
			p = _p;
			q = _q;
		}

		public HexCoor(HexIdx i)
		{
			var _p = 0;
			var _q = 0;

			switch (i)
			{
				case HexIdx.R: _p = 1; _q = 0; break;
				case HexIdx.TR: _p = 0; _q = 1; break;
				case HexIdx.TL: _p = -1; _q = 1; break;
				case HexIdx.L: _p = -1; _q = 0; break;
				case HexIdx.BL: _p = 0; _q = -1; break;
				case HexIdx.BR: _p = 1; _q = -1; break;

				default:
					L.E("invalid idx " + i + ".");
					break;
			}

			p = (HexP)_p;
			q = (HexQ)_q;
		}

		public static HexCoor Round(Vector2 _coor)
		{
			var _q = _coor.y / (SQRT_3 / 2f);
			var _p = _coor.x - _q / 2;
			return new HexCoor((HexP)Mathf.RoundToInt(_p), (HexQ)Mathf.RoundToInt(_q));
		}

		public static IEnumerable<HexCoor> Adjacents()
		{
			foreach (var i in HexHelper.GetIdxes())
				yield return i;
		}

		public IEnumerable<HexCoor> Neighbors()
		{
			foreach (var i in HexHelper.GetIdxes())
				yield return this + i;
		}

		public static HexIdx Side(Vector2 _coor, HexCoor _center)
		{
			var _delta = _coor - _center;
			var _side = Mathf.Atan2(_delta.y, _delta.x) / (Mathf.PI / 3) + 6.5f;
			return (HexIdx) (((int)_side) % 6);
		}

		public override string ToString()
		{
			return "( " + p + ", " + q + " )";
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

		public static implicit operator HexCoor(HexIdx _val)
		{
			return new HexCoor(_val);
		}

		public static explicit operator HexIdx(HexCoor _this)
		{
			if (_this.Compare(HexP.ONE, HexQ.ZERO))
				return HexIdx.R;
			if (_this.Compare(HexP.ZERO, HexQ.ONE))
				return HexIdx.TR;
			if (_this.Compare(HexP.MINUS_ONE, HexQ.ONE))
				return HexIdx.TL;
			if (_this.Compare(HexP.MINUS_ONE, HexQ.ZERO))
				return HexIdx.L;
			if (_this.Compare(HexP.ZERO, HexQ.MINUS_ONE))
				return HexIdx.BL;
			if (_this.Compare(HexP.ONE, HexQ.MINUS_ONE))
				return HexIdx.BR;

			L.E("invalid coor " + _this);
			return HexIdx.R;
		}

		#endregion

		#region arithmetic op
		public override int GetHashCode()
		{
			return p.GetHashCode() ^ q.GetHashCode();
		}

		public static HexCoor operator +(HexCoor _a, HexCoor _b)
		{
			return new HexCoor(_a.p + (int)_b.p, _a.q + (int)_b.q);
		}

		public static HexCoor operator *(HexCoor _this, int _scalar)
		{
			return new HexCoor((HexP)((int)_this.p * _scalar), (HexQ)((int)_this.q * _scalar));
		}

		public static implicit operator Vector2(HexCoor _this)
		{
			return new Vector2((int)_this.p + (int)_this.q / 2f, (int)_this.q * SQRT_3 / 2f);
		}

		#endregion

	}
}