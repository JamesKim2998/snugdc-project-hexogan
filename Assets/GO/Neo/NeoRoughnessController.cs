using System;
using Gem;
using UnityEngine;

namespace HX
{
	public class NeoRoughnessController
	{
		private const float LIVE_THRESHOLD = 0.001f;

		private int mMax;
		public int max
		{
			get { return mMax; }
			private set
			{
				if (mMax == value)
					return;
				D.Assert(value >= 0);
				mMaxOld = mMax;
				mMax = value;
			}
		}
		private int? mMaxOld;

		private float mValue;
		public float value
		{
			get { return mValue; }
			private set
			{
				mValue = Mathf.Clamp(value, 0f, max);
			}
		}

		public int recovery;

		public bool isAlive { get { return value > LIVE_THRESHOLD; } }

		public static Action<int, int> onMaxChanged; 

		public void Update(float _dt)
		{
			Recover(_dt * recovery);

			if (mMaxOld.HasValue)
			{
				onMaxChanged.CheckAndCall(max, mMaxOld.Value);
				mMaxOld = null;
			}
		}

		public void IncreaseMaxAndRecover(int _val)
		{
			max += _val;
			value += _val;
		}

		public void CutMax(int _val)
		{
			var _maxOld = max;
			max -= _val;
			value = value*(max/(float)_maxOld);
		}

		public void Damage(Damage _val)
		{
			value -= _val;
		}

		public void Recover(float _val)
		{
			value += _val;
		}
	}
}