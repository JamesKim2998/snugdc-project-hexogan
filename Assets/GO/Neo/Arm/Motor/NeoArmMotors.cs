using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{
	public class NeoArmMotors
	{
		public readonly NeoRigidbody body;

		#region thrust
		private interface IThrust
		{
			void Add(int _thrust, Vector2 _position);
			void ApplyPos(Rigidbody2D _rigidbody, float _factor);
			void ApplyNeg(Rigidbody2D _rigidbody, float _factor);
			void Clear();
		}

		private struct Thrust : IThrust
		{
			private int mTotal;
			private Vector2 mWeighted;

			public void Add(int _thrust, Vector2 _position)
			{
				mTotal += _thrust;
				mWeighted += _thrust * _position;
			}

			public void ApplyPos(Rigidbody2D _rigidbody, float _factor)
			{
				Apply(_rigidbody, _factor);
			}

			public void ApplyNeg(Rigidbody2D _rigidbody, float _factor)
			{
				Apply(_rigidbody, -_factor);
			}

			private void Apply(Rigidbody2D _rigidbody, float _factor)
			{
				if (mTotal == 0) return;
				var _thrust = _rigidbody.transform.TransformDirection(new Vector2 { x = _factor * mTotal });
				var _position = _rigidbody.transform.TransformPoint(mWeighted / mTotal);
				_rigidbody.AddForceAtPosition(_thrust, _position);
			}

			public void Clear()
			{
				mTotal = 0;
				mWeighted = Vector2.zero;
			}
		}

		private struct Drift : IThrust
		{
			private float mTotal;

			public void Add(int _thrust, Vector2 _position)
			{
				mTotal += _thrust * _position.magnitude;
			}

			public void ApplyPos(Rigidbody2D _rigidbody, float _factor)
			{
				if (Mathf.Approximately(mTotal * _factor, 0)) return;
				_rigidbody.AddTorque(mTotal * _factor);
			}

			public void ApplyNeg(Rigidbody2D _rigidbody, float _factor)
			{
				if (Mathf.Approximately(mTotal * _factor, 0)) return;
				_rigidbody.AddTorque(-mTotal * _factor);
			}

			public void Clear()
			{
				mTotal = 0;
			}
		}
		#endregion

		#region motors
		private struct MotorData
		{
			public NeoArmMotor motor;
			public Vector2 position;
			public HexEdge side { get { return motor.arm.side; } }
		}

		private class MotorDatas
		{
			private readonly List<MotorData> mDatas = new List<MotorData>();
			private readonly IThrust mThrust;

			public MotorDatas(IThrust _thrust)
			{
				mThrust = _thrust;
				isRunning = false;
			}

			public void Add(MotorData _motorData)
			{
				mDatas.Add(_motorData);
			}

			public void ApplyPos(Rigidbody2D _rigidbody, float _factor) { mThrust.ApplyPos(_rigidbody, _factor); }
			public void ApplyNeg(Rigidbody2D _rigidbody, float _factor) { mThrust.ApplyNeg(_rigidbody, _factor); }

			public void BuildThrust(Vector2 _com)
			{
				mThrust.Clear();
				foreach (var _data in mDatas)
					mThrust.Add(_data.motor.thrust, _data.position - _com);
			}

			public void Clear()
			{
				if (isRunning)
					TurnOff();
				mThrust.Clear();
				mDatas.Clear();
			}

			public bool isRunning { get; private set; }

			public void Motor(float _thrust)
			{
				if (!isRunning && _thrust > 0.1f)
					TurnOn();
				else if (isRunning && _thrust < 0.1f)
					TurnOff();
			}

			private void TurnOn()
			{
				if (isRunning)
				{
					Debug.LogWarning("Trying to turn on again. Ignore.");
					return;
				}

				isRunning = true;

				foreach (var _data in mDatas)
					_data.motor.TurnOn();
			}

			private void TurnOff()
			{
				if (!isRunning)
				{
					Debug.LogWarning("Trying to turn off, but motor is not running. Ignore.");
					return;
				}

				isRunning = false;

				foreach (var _data in mDatas)
					_data.motor.TurnOff();
			}
		}

		private readonly List<MotorData> mMotors = new List<MotorData>();
		private readonly MotorDatas mThrusts = new MotorDatas(new Thrust());
		private readonly MotorDatas mDrags = new MotorDatas(new Thrust());
		private readonly MotorDatas mDriftLs = new MotorDatas(new Drift());
		private readonly MotorDatas mDriftRs = new MotorDatas(new Drift());
		#endregion

		public NeoArmMotors(NeoRigidbody _body)
		{
			body = _body;
		}

		#region add/remove

		public void Add(NeoArmMotor _arm, HexCoor _coor)
		{
			var _position = NeoHex.Position(_coor);
			var _motorData = new MotorData { motor = _arm, position = _position, };
			mMotors.Add(_motorData);
		}

		public void Remove(NeoArmMotor _arm)
		{
			mMotors.RemoveAll(_motorData => _motorData.motor == _arm);
		}

		#endregion

		public void ClearThrusts()
		{
			mThrusts.Clear();
			mDrags.Clear();
			mDriftLs.Clear();
			mDriftRs.Clear();
		}

		private const float THRESHOLD = 0.001f;

		public void Motor(float _thrustNormal, float _driftNormal)
		{
			mThrusts.Motor(_thrustNormal);
			mDrags.Motor(-_thrustNormal);
			mDriftLs.Motor(-_driftNormal);
			mDriftRs.Motor(_driftNormal);

			if (body.IsBelowSpeedLimit())
			{
				if (_thrustNormal > THRESHOLD)
					mThrusts.ApplyPos(body, _thrustNormal);
				else if (_thrustNormal < -THRESHOLD)
					mDrags.ApplyNeg(body, -_thrustNormal);
			}

			if (body.IsBelowAngularSpeedLimit())
			{
				if (_driftNormal > THRESHOLD)
					mDriftRs.ApplyNeg(body, _driftNormal);
				else if (_driftNormal < -THRESHOLD)
					mDriftLs.ApplyPos(body, -_driftNormal);
			}
		}

		public void BuildThrust()
		{
			ClearThrusts();

			var _com = body.rigidbody.centerOfMass;

			foreach (var _motorData in mMotors)
			{
				var _side = _motorData.side;
				var _delta = (_motorData.position + NeoHex.Side(_side) / 2) - _com;

				switch (_side)
				{
					case HexEdge.R: mDrags.Add(_motorData); break;
					case HexEdge.L: mThrusts.Add(_motorData); break;

					case HexEdge.TR:
					case HexEdge.TL:
						if (_delta.x < 0)
							mDriftLs.Add(_motorData);
						else if (_delta.x > 0)
							mDriftRs.Add(_motorData);
						break;

					case HexEdge.BR:
					case HexEdge.BL:
						if (_delta.x < 0)
							mDriftRs.Add(_motorData);
						else if (_delta.x > 0)
							mDriftLs.Add(_motorData);
						break;
				}
			}

			mThrusts.BuildThrust(_com);
			mDrags.BuildThrust(_com);
			mDriftLs.BuildThrust(_com);
			mDriftRs.BuildThrust(_com);
		}
	}
}