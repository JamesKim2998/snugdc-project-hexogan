using Gem;
using UnityEngine;

namespace HX.UI
{
	public class NeoViewController : MonoBehaviour
	{
		private const float MAX_WIDTH = 0.75f;
		private const float MAX_HEIGHT = 0.75f;

		private const float ANGULAR_VELOCITY = -15/60f;

		[SerializeField] private Neo mNeo;

		void Update()
		{
			PivotNeo();
		}

		void FixedUpdate()
		{
			transform.SetEulerZ(transform.localEulerAngles.z + ANGULAR_VELOCITY);
		}

		public void PivotNeo()
		{
			var _boundingRect = mNeo.mechanics.boundingRect;
			mNeo.transform.localPosition = -_boundingRect.center;
			
			var _curRatio = _boundingRect.width/_boundingRect.height;
			const float _properRatio = MAX_WIDTH/MAX_HEIGHT;

			if (_curRatio > _properRatio)
			{
				var _scale = MAX_WIDTH/_boundingRect.width;
				transform.SetLScale(_scale < 1 ? _scale : 1);
			}
			else
			{
				var _scale = MAX_HEIGHT / _boundingRect.height;
				transform.SetLScale(_scale < 1 ? _scale : 1);
			}
		}
	}
}