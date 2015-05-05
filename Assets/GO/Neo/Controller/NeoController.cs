using UnityEngine;

namespace HX
{
	public class NeoController : MonoBehaviour
	{
		public Neo neo;

		private const float INPUT_THRESHHOLD = 0.01f;
		private const string INPUT_VERTICAL = "Vertical";
		private const string INPUT_HORIZONTAL = "Horizontal";

		static float CutDeadzone(float _var)
		{
			const float _scaler = 1 / (1 - INPUT_THRESHHOLD);
			if (_var > INPUT_THRESHHOLD)
				return _scaler * (_var - INPUT_THRESHHOLD);
			if (_var < -INPUT_THRESHHOLD)
				return _scaler * (_var + INPUT_THRESHHOLD);
			return 0;
		}

		void FixedUpdate()
		{
			if (!neo) return;

			var _vertical = Input.GetAxis(INPUT_VERTICAL);
			var _horizontal = Input.GetAxis(INPUT_HORIZONTAL);
			_vertical = CutDeadzone(_vertical);
			_horizontal = CutDeadzone(_horizontal);

			var _shouldMotor = !(Mathf.Approximately(_vertical, 0)
				&& Mathf.Approximately(_horizontal, 0));

			if (_shouldMotor)
				neo.Motor(Time.fixedDeltaTime, _vertical, _horizontal);

			if (Input.GetButtonDown(NeoInput.SHOOT))
				neo.Shoot();
		}
	}
}