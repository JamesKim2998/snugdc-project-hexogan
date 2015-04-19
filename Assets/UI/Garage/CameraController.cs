using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera mUI;
		[SerializeField] private Camera mWorld;

		public Camera ui { get { return mUI; } }
		public Camera world { get { return mWorld; } }

		public Neo neo { private get; set; }

		private bool mIsRiveted = true;

		private void Update()
		{
			if (mIsRiveted)
				UpdateRiveted();
			else
				UpdateFollow();
		}

		private float GetNeoSize()
		{
			var _rect = neo.mechanics.boundingRect;
			return Mathf.Max(_rect.width, _rect.height);
		}

		private float GetRivetedCameraSize()
		{
			return GetNeoSize();
		}

		private float GetFollowCameraSize()
		{
			var _velocity = neo.body.rigidbody.velocity;
			var _sizeByVelocity = _velocity.magnitude;
			return 2 * GetNeoSize() + _sizeByVelocity/2;
		}

		private void UpdateRiveted()
		{
			var _dt = Time.deltaTime;
			var _lerp = 4 * _dt;
			mWorld.orthographicSize = Mathf.Lerp(mWorld.orthographicSize, GetRivetedCameraSize(), _lerp);
		}

		private void UpdateFollow()
		{
			var _dt = Time.deltaTime;
			var _lerp = 4 * _dt;

			mWorld.orthographicSize = Mathf.Lerp(mWorld.orthographicSize, GetFollowCameraSize(), _lerp);

			var _neoTrans = neo.transform;
			var _camTrans = mWorld.transform;

			var _newPos = Vector2.Lerp(_camTrans.position, _neoTrans.position, _lerp);
			_camTrans.SetPos(_newPos);

			var _angleLerp = 2*_dt;
			var _newZ = Mathf.LerpAngle(_camTrans.eulerAngles.z, _neoTrans.eulerAngles.z, _angleLerp);
			_camTrans.SetEulerZ(_newZ);
		}

		public Vector2 WorldToUI(Vector2 _globalPos)
		{
			var _viewport = world.WorldToViewportPoint(_globalPos);
			return ui.ViewportToWorldPoint(_viewport);
		}

		public Vector2 UIToWorld(Vector2 _globalPos)
		{
			var _viewport = ui.WorldToViewportPoint(_globalPos);
			return world.ViewportToWorldPoint(_viewport);
		}

		public Vector2 GetGlobalMousePosition(Vector2 _offset)
		{
			var _screenPos = Input.mousePosition + (Vector3)_offset;
			return world.ScreenToWorldPoint(_screenPos);
		}

		private void ResetCamera()
		{
			mWorld.orthographicSize = GetRivetedCameraSize();
			mWorld.transform.SetLEulerZ(0);
			mWorld.transform.SetLPos(Vector2.zero);
		}

		public void ZoomIn()
		{
			mIsRiveted = false;
			ResetCamera();
		}

		public void ZoomOut()
		{
			mIsRiveted = true;
			ResetCamera();
		}
	}
}