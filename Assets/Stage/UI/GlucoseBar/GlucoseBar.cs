using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public class GlucoseBar : MonoBehaviour
	{
		private const float THRESHHOLD = 0.1f;
		private const float LENGTH_MAX = 1000;
		private const float VALUE_TO_LEN = 1;

		private float mValue;
		public float value
		{
			get { return mValue; }
			set
			{
				if (Mathf.Abs(this.value - value) < THRESHHOLD)
					return;

				mValue = value;
				Refresh();
			}
		}

		private int mMax;
		public int max
		{
			get { return mMax; }
			set
			{
				mMax = value;
				mBar.width = (int)Mathf.Min(value * VALUE_TO_LEN, LENGTH_MAX);
				Refresh();
			}
		}

		[SerializeField, UsedImplicitly] private UISprite mBar;
		[SerializeField, UsedImplicitly] private UIProgressBar mProgress;

		void Refresh()
		{
			if (max != 0)
				mProgress.value = value / max;
		}
	}
}