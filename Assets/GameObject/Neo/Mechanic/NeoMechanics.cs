using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;

namespace HX
{
	public class NeoMechanics : MonoBehaviour
	{
		private NeoRigidbody m_Body;
		public NeoRigidbody body
		{
			get { return m_Body; }
			set
			{
				if (body == value) return;
				m_Body = value;
				motors.body = body;
				emitters.neoBody = body;
			}
		}

		public NeoArmMotors motors { get; private set; }
		public NeoArmEmitters emitters { get; private set; }

		private readonly HexGrid<NeoBody> m_Bodies = new HexGrid<NeoBody>();
		private readonly List<NeoArm> m_Arms = new List<NeoArm>();

		private bool m_MassDirty = false;
		private bool m_IslandDirty = false;

		void Awake()
		{
			motors = gameObject.AddComponent<NeoArmMotors>();
			emitters = gameObject.AddComponent<NeoArmEmitters>();
		}

		void OnDestroy()
		{
			Destroy(motors);
			Destroy(emitters);
		}

		void Update()
		{
			if (m_IslandDirty)
			{
				var _island = m_Bodies.RemoveIslands();
				foreach (var _cell in _island)
					Remove(_cell.data, false);
				m_IslandDirty = false;
			}

			if (m_MassDirty)
			{
				Build();
				m_MassDirty = false;
			}
		}

		public HexCell<NeoBody> GetBody(HexCoor _coor)
		{
			HexCell<NeoBody> _body;
			return m_Bodies.TryGet(_coor, out _body)
				? _body
				: null;
		}

		void Add(NeoMechanic _mechanic, HexCoor _coor, HexIdx? _side = null)
		{
			_mechanic.SetParent(this, _coor);
			body.AddMass(_mechanic.mass, _coor, _side);
			m_MassDirty = true;
		}

		public bool IsRemovable(NeoMechanic _mechanic)
		{
			return _mechanic.parent == this;
		}

		public void Remove(NeoMechanic _mechanic)
		{
			var _body = _mechanic.GetComponent<NeoBody>();
			if (_body) { Remove(_body, true); return; }

			var _arm = _mechanic.GetComponent<NeoArm>();
			if (_arm) { Remove(_arm); return; }
		}

		public bool Add(NeoBody _body, HexCoor _coor)
		{
			var _cell = new HexCell<NeoBody>(_body);
			if (!m_Bodies.TryAdd(_coor, _cell))
				return false;

			Add((NeoMechanic)_body, _coor);

			_body.transform.parent = transform;
			Locate(_body.transform, _coor);

			var _side = -1;
			foreach (var _neighbor in _cell.GetNeighbors())
			{
				++_side;
				if (_neighbor == null) continue;
				_body.AddBody(_neighbor.data, (HexIdx)_side);
			}

			return true;
		}

		public void Add(NeoArm _arm, HexCoor _coor, HexIdx _side)
		{
			var _body = m_Bodies[_coor];
			if (_body == null) return;

			Add((NeoMechanic)_arm, _coor, _side);

			_body.data.AddArm(_arm, _side);
			m_Arms.Add(_arm);

			var _motor = _arm.GetComponent<NeoArmMotor>();
			if (_motor) motors.Add(_motor, _coor);

			var _emitter = _arm.GetComponent<NeoArmEmitter>();
			if (_emitter) emitters.Add(_emitter);
		}

		private void RemoveMechanic(NeoMechanic _mechanic, HexCoor _coor, HexIdx? _side = null)
		{
			_mechanic.Detach();
			body.AddMass(-_mechanic.mass, _coor, _side);
			m_MassDirty = true;
		}

		public void Remove(NeoArm _arm)
		{
			if (!IsRemovable(_arm)) return;

			var _motor = _arm.GetComponent<NeoArmMotor>();
			if (_motor) motors.Remove(_motor);

			var _emitter = _arm.GetComponent<NeoArmEmitter>();
			if (_emitter) emitters.Remove(_emitter);

			m_Arms.Remove(_arm);
			RemoveMechanic(_arm, _arm.coor, _arm.side);
		}

		public void Remove(NeoBody _body, bool _removeFromGrid = true)
		{
			if (!IsRemovable(_body)) return;

			var _isCore = _body.coor == HexCoor.ZERO;

			if (_removeFromGrid)
			{
				var _cell = GetBody(_body.coor);
				if (_cell == null)
				{
					Debug.LogError("Cell is not exists! Ignore.");
					return;
				}

				m_IslandDirty |= _cell.IsBridge();
				m_Bodies.Remove(_body.coor);
			}

			RemoveMechanic(_body, _body.coor);

			if (_isCore) RemoveAll();
		}

		private void RemoveAll()
		{
			var _arms = new List<NeoArm>(m_Arms);
			foreach (var _arm in _arms)
				Remove(_arm);

			var _bodies = m_Bodies.ToList();
			foreach (var _body in _bodies)
				Remove(_body.Value.data, false);

			m_Bodies.Clear();
			Destroy(this);
		}

		public void Build()
		{
			motors.BuildThrust();
		}

		public static void Locate(Transform _transform, HexCoor _coor)
		{
			var _pos = (Vector3)NeoHex.Position(_coor);
			var _posOld = _transform.localPosition;
			_pos.z = _posOld.z;
			_transform.localPosition = _pos;
		}

	}
}