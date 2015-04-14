using System;
using Gem;
using UnityEngine;

namespace HX.UI
{
	public class NeoTransitionAnimator : MonoBehaviour
	{
		private const float DURATION = 1f;

		private bool mIsIn;
		private float mTimer;
		private Action mOnDone;

		[SerializeField] private Camera mUICamera;
		[SerializeField] private Camera mWorldCamera;

		[SerializeField] private GameObject mUIMainRoot;
		[SerializeField] private GameObject mUIGarageRoot;
		[SerializeField] private GameObject mWorldMainRoot;
		[SerializeField] private GameObject mWorldGarageRoot;

		[SerializeField] private Vector2 mMainCenter;
		[SerializeField] private Vector2 mGarageCenter;

		Vector2 GetCameraPosition()
		{
			return mUICamera.transform.localPosition;
		}

		void SetCameraPosition(Vector2 _pos)
		{
			mUICamera.transform.SetLPos(_pos);
			var _sizeRatio = mWorldCamera.orthographicSize/mUICamera.orthographicSize;
			var _ratio = 4f/Const.RESOLUTION_Y*_sizeRatio;
			mWorldCamera.transform.SetLPos(_pos*_ratio);
		}

		void MoveCameraBy(Vector2 _delta)
		{
			SetCameraPosition(GetCameraPosition() + _delta);
		}

		void SetMainActive(bool _val)
		{
			mUIMainRoot.SetActive(_val);
			mWorldMainRoot.SetActive(_val);
		}

		void SetGarageActive(bool _val)
		{
			mUIGarageRoot.SetActive(_val);
			mWorldGarageRoot.SetActive(_val);
		}

		void Update()
		{
			if (mIsIn && mTimer > 0)
			{
				UpdateIn();
			}

			else if (!mIsIn && mTimer > 0)
			{
				UpdateOut();
			}
		}

		void UpdateIn()
		{
			var _dt = Time.deltaTime;
			var _oldTimer = mTimer;
			mTimer -= _dt;
			var _meanTimer = (mTimer + _oldTimer)/2;

			if (mTimer < 0)
			{
				SetCameraPosition(mGarageCenter);
				SetMainActive(false);
				mOnDone.CheckAndCall();
			}
			else
			{
				var _dtNormal = _dt / _meanTimer;
				var _uiDelta = mGarageCenter - GetCameraPosition();
				MoveCameraBy(_uiDelta * _dtNormal);
			}
		}

		void UpdateOut()
		{
			
		}

		public bool PlayIn(Action _onDone = null)
		{
			if (mIsIn)
			{
				L.W("call again.");
				return false;
			}
			
			mTimer = DURATION;
			mIsIn = true;
			mOnDone = _onDone;
			SetGarageActive(true);
			return true;
		}

		public bool PlayOut(Action _onDone = null)
		{
			if (!mIsIn)
			{
				L.W("call again.");
				return false;
			}

			mTimer = DURATION;
			mIsIn = false;
			mOnDone = _onDone;
			return true;
		}
	}
}
