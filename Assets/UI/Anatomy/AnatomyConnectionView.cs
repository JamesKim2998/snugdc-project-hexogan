using Gem;
using UnityEngine;

namespace HX.UI
{
	public class AnatomyConnectionView : MonoBehaviour
	{
		[SerializeField] private UISprite mSprite;
		[SerializeField] private UISprite mCommute;

		public void Pivot(Vector2 a, Vector2 b)
		{
			var _delta = a - b;
			transform.localPosition = (a + b) / 2;
			transform.SetLEulerZ(_delta.ToDeg());
			mSprite.width = (int)_delta.magnitude;
		}

		public void SetLock(bool _val)
		{
			mSprite.color = _val ? Color.green : Color.red;
		}

		public void Highlight(bool _val)
		{
			mCommute.gameObject.SetActive(_val);

			if (_val)
				mCommute.transform.localPosition = new Vector2(0, (float)mSprite.height/2);

			// todo: animation
		}
	}

}