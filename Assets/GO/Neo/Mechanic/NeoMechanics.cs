using System;
using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;

namespace HX
{
	public class NeoMechanics
	{
		public readonly Neo neo;
		public NeoRigidbody body { get { return neo.body; } }
		public readonly NeoArmMotors motors;
		public readonly NeoArmEmitters emitters;

		public Transform transform { get { return neo.transform; } }

		public Rect boundingRect { get; private set; }

		private readonly HexGraph<NeoBody> mBodies = new HexGraph<NeoBody>();
		private readonly List<NeoArm> mArms = new List<NeoArm>();

		private bool mMassDirty = false;
		private bool mIslandDirty = false;
		private readonly List<NeoMechanic> mMechanicsDirty = new List<NeoMechanic>();

		public NeoMechanics(Neo _neo)
		{
			neo = _neo;
			motors = new NeoArmMotors(body);
			emitters = new NeoArmEmitters(neo.GetInstanceID(), body);
		}

		public void Update()
		{
			foreach (var _mechanic in mMechanicsDirty)
				_mechanic.collider.enabled = true;
			mMechanicsDirty.Clear();
			
			if (mIslandDirty)
			{
				var _island = mBodies.RemoveIslands();
				foreach (var _node in _island)
					Remove(_node.data, false);
				mIslandDirty = false;
			}

			if (mMassDirty)
			{
				Build();
				mMassDirty = false;
			}
		}

		public HexNode<NeoBody> GetBody(HexCoor _coor)
		{
			HexNode<NeoBody> _body;
			return mBodies.TryGet(_coor, out _body)
				? _body
				: null;
		}

		void Add(NeoMechanic _mechanic, HexCoor _coor, HexEdge? _side = null)
		{
			_mechanic.collider.enabled = false;
			_mechanic.SetParent(this, _coor);
			body.AddMass(_mechanic.mass, _coor, _side);
			mMassDirty = true;
			mMechanicsDirty.Add(_mechanic);
		}

		public bool IsRemovable(NeoMechanic _mechanic)
		{
			return _mechanic.parent == this;
		}

		public void Remove(NeoMechanic _mechanic)
		{
			if (_mechanic == NeoMechanicType.BODY)
			{
				var _body = _mechanic.GetComponent<NeoBody>();
				Remove(_body, true);
			}
			else if (_mechanic == NeoMechanicType.ARM)
			{
				var _arm = _mechanic.GetComponent<NeoArm>();
				Remove(_arm);
			}
			else
			{
				L.E("undefined mechanic type " + _mechanic.mechanicType);
			}
		}

		private void BeforeRemove(NeoMechanic _mechanic)
		{
			mMechanicsDirty.Remove(_mechanic);
		}

		public bool Add(NeoBody _body, HexCoor _coor)
		{
			var _cell = new HexNode<NeoBody>(_body);
			if (!mBodies.TryAdd(_coor, _cell))
				return false;

			Add((NeoMechanic)_body, _coor);

			_body.transform.SetParent(transform, false);
			Locate(_body.transform, _coor);

			var _side = -1;
			foreach (var _neighbor in _cell.GetAdjacents())
			{
				++_side;
				if (_neighbor == null) continue;
				_body.AddBody(_neighbor.data, (HexEdge)_side);
			}

			return true;
		}

		public void Add(NeoArm _arm, HexCoor _coor, HexEdge _side)
		{
			var _body = mBodies[_coor];
			if (_body == null) return;

			Add((NeoMechanic)_arm, _coor, _side);

			_body.data.AddArm(_arm, _side);
			mArms.Add(_arm);

			var _motor = _arm.GetComponent<NeoArmMotor>();
			if (_motor) motors.Add(_motor, _coor);

			var _emitter = _arm.GetComponent<NeoArmEmitter>();
			if (_emitter) emitters.Add(_emitter);
		}

		private void RemoveMechanic(NeoMechanic _mechanic, HexCoor _coor, HexEdge? _side = null)
		{
			_mechanic.Detach();
			body.AddMass(-_mechanic.mass, _coor, _side);
			mMassDirty = true;
		}

		public void Remove(NeoArm _arm)
		{
			if (!IsRemovable(_arm)) return;

			BeforeRemove(_arm);

			switch (_arm.type)
			{
				case NeoArmType.MOTOR:
					var _motor = _arm.GetComponent<NeoArmMotor>();
					motors.Remove(_motor);
					break;
				case NeoArmType.EMITTER:
					var _emitter = _arm.GetComponent<NeoArmEmitter>();
					emitters.Remove(_emitter);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			mArms.Remove(_arm);
			RemoveMechanic(_arm, _arm.coor, _arm.side);
		}

		public void Remove(NeoBody _body, bool _removeFromGrid = true)
		{
			if (!IsRemovable(_body)) return;

			BeforeRemove(_body);

			var _isCore = _body.coor == HexCoor.ZERO;

			if (_removeFromGrid)
			{
				var _cell = GetBody(_body.coor);
				if (_cell == null)
				{
					Debug.LogError("cell is not exists! ignore.");
					return;
				}

				mIslandDirty |= _cell.IsBridge();
				mBodies.Remove(_body.coor);
			}

			RemoveMechanic(_body, _body.coor);

			if (_isCore) RemoveAll();
		}

		private void RemoveAll()
		{
			var _arms = new List<NeoArm>(mArms);
			foreach (var _arm in _arms)
				Remove(_arm);

			var _bodies = mBodies.ToList();
			foreach (var _body in _bodies)
				Remove(_body.Value.data, false);

			mBodies.Clear();
			mMechanicsDirty.Clear();
		}

		public void Build(NeoBlueprint _structure)
		{
			foreach (var _body in _structure.GetBodyEnum())
			{
				var _bodyGO = _body.assembly.staticData.MakeBody();
				Add(_bodyGO, _body.coor);
			}

			foreach (var _arm in _structure.GetArmEnum())
			{
				var _armGO = _arm.assembly.staticData.MakeArm();
				Add(_armGO, _arm.coor, _arm.side);
			}

			Build();
		}

		private struct BoundingRectMaker
		{
			private Rect mRect;
			public Rect rect { get { return mRect; } }

			public void Add(Vector2 p)
			{
				const float _xMargin = NeoConst.HEX_P / 2;
				const float _yMargin = 1.5f * NeoConst.HEX_SIDE;

				if (p.x - _xMargin < rect.xMin)
					mRect.xMin = p.x - _xMargin;
				if (p.x + _xMargin > rect.xMax)
					mRect.xMax = p.x + _xMargin;

				if (p.y - _yMargin < rect.yMin)
					mRect.yMin = p.y - _yMargin;
				if (p.y + _yMargin > rect.yMax)
					mRect.yMax = p.y + _yMargin;
			}
		}

		public void Build()
		{
			var _boundingRectMaker = new BoundingRectMaker();
			foreach (var kv in mBodies)
				_boundingRectMaker.Add(NeoHex.Position(kv.Key));
			boundingRect = _boundingRectMaker.rect;

			motors.BuildThrust();
		}

		public static void Locate(Transform _transform, HexCoor _coor)
		{
			var _pos = (Vector3)NeoHex.Position(_coor);
			var _posOld = _transform.localPosition;
			_pos.z = _posOld.z;
			_transform.localPosition = _pos;
		}

		public static implicit operator bool(NeoMechanics _this)
		{
			return _this != null;
		}
	}
}