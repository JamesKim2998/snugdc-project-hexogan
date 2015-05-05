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
				var _thrust = _rigidbody.transform.TransformVector(new Vector2 { x = _factor * mTotal });
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
			private int mConsumption;

			public MotorDatas(IThrust _thrust)
			{
				mThrust = _thrust;
				isRunning = false;
			}

			public void Add(MotorData _motorData)
			{
				mDatas.Add(_motorData);
				mConsumption += _motorData.motor.thrustEnergyConsumption;
			}

			public float CalculateConsumption(float _dt, float _factor)
			{
				return _dt*_factor*mConsumption;
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
				mConsumption = 0;
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

		private class MotorDatasComplementary
		{
			public readonly MotorDatas pos;
			public readonly MotorDatas neg;

			public MotorDatasComplementary(MotorDatas _pos, MotorDatas _neg)
			{
				pos = _pos;
				neg = _neg;
			}

			public float CalculateConsumption(float _dt, float _factor)
			{
				return (_factor > 0)
					? pos.CalculateConsumption(_dt, _factor) 
					: neg.CalculateConsumption(_dt, -_factor);
			}

			public void BuildThrust(Vector2 _com)
			{
				pos.BuildThrust(_com);
				neg.BuildThrust(_com);
			}

			public void Clear()
			{
				pos.Clear(); 
				neg.Clear();
			}

			public void Motor(float _val)
			{
				pos.Motor(_val);
				neg.Motor(-_val);
			}

			public void ApplyForce(NeoRigidbody _body, float _val)
			{
				if (_val > 0)
					pos.ApplyPos(_body, _val);
				else
					neg.ApplyNeg(_body, -_val);
			}
		}

		private readonly List<MotorData> mMotors = new List<MotorData>();
		private readonly MotorDatasComplementary mThrusts = new MotorDatasComplementary(new MotorDatas(new Thrust()), new MotorDatas(new Thrust()));
		private readonly MotorDatasComplementary mDrifts = new MotorDatasComplementary(new MotorDatas(new Drift()), new MotorDatas(new Drift()));
		#endregion

		private readonly NeoEnergyController mEnergyController;

		public NeoArmMotors(NeoRigidbody _body, NeoEnergyController _energyController)
		{
			body = _body;
			mEnergyController = _energyController;
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
			mDrifts.Clear();
		}

		public void Motor(float _dt, float _thrustNormal, float _driftNormal)
		{
			var _shouldThrust = body.IsBelowSpeedLimit();
			var _shouldDrift = body.IsBelowAngularSpeedLimit();

			var _consumption = 0f;
			if (_shouldThrust)
				_consumption += mThrusts.CalculateConsumption(_dt, _thrustNormal);
			if (_shouldDrift)
				_consumption += mDrifts.CalculateConsumption(_dt, _driftNormal);

			if (!mEnergyController.TryConsume(_consumption))
				return;

			mThrusts.Motor(_thrustNormal);
			mDrifts.Motor(_driftNormal);

			if (_shouldThrust)
				mThrusts.ApplyForce(body, _thrustNormal);
			if (_shouldDrift)
				mDrifts.ApplyForce(body, _driftNormal);
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
					case HexEdge.R: mThrusts.neg.Add(_motorData); break;
					case HexEdge.L: mThrusts.pos.Add(_motorData); break;

					case HexEdge.TR:
					case HexEdge.TL:
						if (_delta.x < 0)
							mDrifts.neg.Add(_motorData);
						else if (_delta.x > 0)
							mDrifts.pos.Add(_motorData);
						break;

					case HexEdge.BR:
					case HexEdge.BL:
						if (_delta.x < 0)
							mDrifts.pos.Add(_motorData);
						else if (_delta.x > 0)
							mDrifts.neg.Add(_motorData);
						break;
				}
			}

			mThrusts.BuildThrust(_com);
			mDrifts.BuildThrust(_com);
		}
	}
}