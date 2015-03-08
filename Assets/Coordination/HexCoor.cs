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

		public int p, q;

		public HexCoor(int _p, int _q)
		{
			p = _p;
			q = _q;
		}

		public IEnumerable<HexCoor> Neighbors()
		{
			for (var _i = 0; _i != 6; ++_i)
			{
				yield return this + FromAdjacent(_i);
			}
		}

		public static int OppositeSide(int _index)
		{
			return (_index + 3) % 6;
		}

		public int ToAdjacent()
		{
			if (Compare(1, 0))
				return 0;
			if (Compare(0, 1))
				return 1;
			if (Compare(-1, 1))
				return 2;
			if (Compare(-1, 0))
				return 3;
			if (Compare(0, -1))
				return 4;
			if (Compare(1, -1))
				return 5;

			Debug.LogError("Invalid direction " + this + ".");
			return -1;
		}

		public static IEnumerable<HexCoor> GetAdjacents()
		{
			for (var _i = 0; _i < 6; ++_i)
				yield return FromAdjacent(_i);
		}

		public static HexCoor FromAdjacent(int _idx)
		{
			switch (_idx)
			{
				case 0: return new HexCoor(1, 0);
				case 1: return new HexCoor(0, 1);
				case 2: return new HexCoor(-1, 1);
				case 3: return new HexCoor(-1, 0);
				case 4: return new HexCoor(0, -1);
				case 5: return new HexCoor(1, -1);
				default:
					Debug.LogError("Invalid index " + _idx + ".");
					return new HexCoor(0, 0);
			}
		}

		public static HexCoor Round(Vector2 _coor)
		{
			var _q = _coor.y / (SQRT_3 / 2f);
			var _p = _coor.x - _q / 2;
			return new HexCoor(Mathf.RoundToInt(_p), Mathf.RoundToInt(_q));
		}

		public static int Side(Vector2 _coor, HexCoor _center)
		{
			var _delta = _coor - _center;
			var _side = Mathf.Atan2(_delta.y, _delta.x) / (Mathf.PI / 3) + 6.5f;
			return ((int)_side) % 6;
		}

		public override string ToString()
		{
			return "( " + p + ", " + q + " )";
		}

		#region equality op
		public bool Compare(int _p, int _q)
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

		public static bool operator ==(HexCoor _self, HexCoor _other)
		{
			return _self.p == _other.p
				   && _self.q == _other.q;
		}

		public static bool operator !=(HexCoor _self, HexCoor _other)
		{
			return !(_self == _other);
		}
		#endregion

		#region arithmetic op
		public override int GetHashCode()
		{
			return p.GetHashCode() ^ q.GetHashCode();
		}

		public static HexCoor operator +(HexCoor _a, HexCoor _b)
		{
			return new HexCoor(_a.p + _b.p, _a.q + _b.q);
		}

		public static HexCoor operator *(HexCoor _coor, int _scalar)
		{
			return new HexCoor(_coor.p * _scalar, _coor.q * _scalar);
		}

		public static implicit operator Vector2(HexCoor _self)
		{
			return new Vector2(_self.p + _self.q / 2f, _self.q * SQRT_3 / 2f);
		}

		#endregion
	}
}