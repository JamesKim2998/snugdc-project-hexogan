using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NeoArmMotors : MonoBehaviour
{
	public NeoRigidbody body;

	#region thrust
	private interface IThrust
	{
		void Add(int _thrust, Vector2 _position);
		void ApplyPos(Rigidbody2D _rigidbody, float _factor);
		void ApplyNeg(Rigidbody2D _rigidbody, float _factor);
		void Clear();
	}

	private class Thrust : IThrust
	{
		private int m_Total;
		private Vector2 m_Weighted = Vector2.zero;
		public void Add(int _thrust, Vector2 _position)
		{
			m_Total += _thrust;
			m_Weighted += _thrust * _position;
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
			if (m_Total == 0) return;
			var _thrust = _rigidbody.transform.TransformDirection(new Vector2 { x = _factor * m_Total });
			var _delta = _rigidbody.transform.TransformDirection(m_Weighted / m_Total);
			_rigidbody.AddForceAtPosition(_thrust, _rigidbody.transform.localPosition + _delta);
		}

		public void Clear()
		{
			m_Total = 0;
			m_Weighted = Vector2.zero;
		}
	}

	private class Drift : IThrust
	{
		private float m_Total;

		public void Add(int _thrust, Vector2 _position)
		{
			m_Total += _thrust * _position.magnitude;
		}

		public void ApplyPos(Rigidbody2D _rigidbody, float _factor)
		{
			if (Mathf.Approximately(m_Total * _factor, 0)) return;
			_rigidbody.AddTorque(m_Total * _factor);
		}

		public void ApplyNeg(Rigidbody2D _rigidbody, float _factor)
		{
			if (Mathf.Approximately(m_Total * _factor, 0)) return;
			_rigidbody.AddTorque(-m_Total * _factor);
		}

		public void Clear()
		{
			m_Total = 0;
		}
	}
	#endregion

	#region motors
	private struct MotorData
	{
		public NeoArmMotor motor;
		public Vector2 position;
		public int side { get { return motor.GetComponent<NeoArm>().side;  } }
	}

	private class MotorDatas
	{
		private readonly List<MotorData> m_Datas = new List<MotorData>();
		private readonly IThrust m_Thrust;

		public MotorDatas(IThrust _thrust)
		{
			m_Thrust = _thrust;
			isRunning = false;
		}

		public void Add(MotorData _motorData)
		{
			m_Datas.Add(_motorData);
		}

		public void ApplyPos(Rigidbody2D _rigidbody, float _factor) { m_Thrust.ApplyPos(_rigidbody, _factor); }
		public void ApplyNeg(Rigidbody2D _rigidbody, float _factor) { m_Thrust.ApplyNeg(_rigidbody, _factor); }

		public void BuildThrust(Vector2 _com)
		{
			m_Thrust.Clear();
			foreach (var _data in m_Datas)
				m_Thrust.Add(_data.motor.thrust, _data.position - _com);
		}

		public void Clear()
		{
			if (isRunning) 
				TurnOff();
			m_Thrust.Clear();
			m_Datas.Clear();
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

			foreach (var _data in m_Datas)
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

			foreach (var _data in m_Datas)
				_data.motor.TurnOff();
		}
	}

	private readonly List<MotorData> m_Motors = new List<MotorData>();
	private readonly MotorDatas m_Thrusts = new MotorDatas(new Thrust());
	private readonly MotorDatas m_Drags = new MotorDatas(new Thrust());
	private readonly MotorDatas m_DriftLs = new MotorDatas(new Drift());
	private readonly MotorDatas m_DriftRs = new MotorDatas(new Drift());
	#endregion

	#region add/remove

	public void Add(NeoArmMotor _motor, HexCoor _coor)
	{
		var _position = NeoHex.Position(_coor);
		var _motorData = new MotorData {motor = _motor, position = _position, };
		m_Motors.Add(_motorData);
	}

	public void Remove(NeoArmMotor _motor)
	{
		m_Motors.RemoveAll(_motorData => _motorData.motor == _motor);
	}

	#endregion

	public void ClearThrusts()
	{
		m_Thrusts.Clear();
		m_Drags.Clear();
		m_DriftLs.Clear();
		m_DriftRs.Clear();
	}

	public void Motor(float _thrustNormal, float _driftNormal)
	{
		m_Thrusts.Motor(_thrustNormal);
		m_Drags.Motor(-_thrustNormal);
		m_DriftLs.Motor(-_driftNormal);
		m_DriftRs.Motor(_driftNormal);

		if (rigidbody2D.velocity.sqrMagnitude < body.speedLimit * body.speedLimit)
		{
			if (_thrustNormal > 0.001f)
				m_Thrusts.ApplyPos(rigidbody2D, _thrustNormal);
			else if (_thrustNormal < -0.001f)
				m_Drags.ApplyNeg(rigidbody2D, -_thrustNormal);
		}

		if (Mathf.Abs(rigidbody2D.angularVelocity) < body.angularSpeedLimit)
		{
			if (_driftNormal > 0.001f)
				m_DriftRs.ApplyNeg(rigidbody2D, _driftNormal);
			else if (_driftNormal < -0.001f)
				m_DriftLs.ApplyPos(rigidbody2D, -_driftNormal);
		}
	}

	public void BuildThrust()
	{
		ClearThrusts();

		var _com = body.rigidbody2D.centerOfMass;

		foreach (var _motorData in m_Motors)
		{
			var _side = _motorData.side;
			var _delta = (_motorData.position + NeoHex.Side(_side) / 2) - _com;

			switch (_side)
			{
				case 0: m_Drags.Add(_motorData); break;
				case 3: m_Thrusts.Add(_motorData); break;

				case 1:
				case 2:
					if (_delta.x < 0)
						m_DriftLs.Add(_motorData);
					else if (_delta.x > 0)
						m_DriftRs.Add(_motorData);
					break;

				case 4:
				case 5:
					if (_delta.x < 0)
						m_DriftRs.Add(_motorData);
					else if (_delta.x > 0)
						m_DriftLs.Add(_motorData);
					break;
			}
		}

		m_Thrusts.BuildThrust(_com);
		m_Drags.BuildThrust(_com);
		m_DriftLs.BuildThrust(_com);
		m_DriftRs.BuildThrust(_com);
	}
}
