using JetBrains.Annotations;
using UnityEngine;

namespace HX.Stage
{
	public class BaseBar : MonoBehaviour
	{
		private const float THRESHHOLD = 0.1f;
		
		public float lengthMax { get; protected set; }
		public float valueToLength { get; protected set; }

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
				var _width = (int)Mathf.Min(value * valueToLength, lengthMax);
				mBar.width = _width;
				mBack.width = _width;
				Refresh();
			}
		}

		[SerializeField, UsedImplicitly] 
		private UISprite mBar;

		[SerializeField, UsedImplicitly]
		private UISprite mBack;

		[SerializeField, UsedImplicitly]
		private UIProgressBar mProgress;

		public BaseBar()
		{
			lengthMax = 800;
			valueToLength = 1;
		}

		void Refresh()
		{
			if (max != 0)
				mProgress.value = value / max;
		}
	}
}