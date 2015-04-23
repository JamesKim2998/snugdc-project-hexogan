using System;
using System.Collections.Generic;
using System.Linq;
using Gem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HX
{
	public class NeoBlueprint
	{
		[Serializable]
		public struct Body
		{
			[JsonIgnore]
			public NeoBodyType type { get { return assembly.type; } }

			public HexCoor coor;

			public AssemblyID assemblyID;

			[JsonIgnore]
			private BodyAssembly mAssembly;
			[JsonIgnore]
			public BodyAssembly assembly
			{
				get { return mAssembly; }
				set
				{
					D.Assert(assemblyID == default(AssemblyID)
						|| assemblyID == value);
					assemblyID = value;
					mAssembly = value;
				}
			}
		}

		[Serializable]
		public struct Arm
		{
			[JsonIgnore]
			public NeoArmType type { get { return assembly.type; } }

			public HexCoor bodyCoor;
			public HexEdge side;

			public HexCoor armCoor { get { return bodyCoor + side; } }

			public AssemblyID assemblyID;

			[JsonIgnore]
			private ArmAssembly mAssembly;
			[JsonIgnore]
			public ArmAssembly assembly
			{
				get { return mAssembly; }
				set
				{
					D.Assert(assemblyID == default(AssemblyID) 
						|| assemblyID == value);
					assemblyID = value;
					mAssembly = value;
				}
			}
		}

		private readonly HexGraph<Body> mBodies = new HexGraph<Body>();
		private readonly HexGraph<Arm> mArms = new HexGraph<Arm>();

		private readonly Dictionary<AssemblyID, HexCoor> mBodyToCoor = new Dictionary<AssemblyID, HexCoor>();
		private readonly Dictionary<AssemblyID, HexCoor> mArmToCoor = new Dictionary<AssemblyID, HexCoor>(); 

		public NeoBlueprint()
		{
			mBodies.allowIsland = false;
			mArms.allowIsland = true;
		}

		private static bool BeforeAdd(Assembly _assembly)
		{
			if (!_assembly.availiable)
			{
				L.E("assembly is not available.");
				return false;
			}

			return true;
		}

		private static void MarkAvailable(Assembly _assembly)
		{
			_assembly.availiable = true;
		}

		private static void MarkNotAvailable(Assembly _assembly)
		{
			_assembly.availiable = false;
		}

		public bool TryAdd(HexCoor _coor, BodyAssembly _assembly, bool _checkIsland = true)
		{
			if (!BeforeAdd(_assembly))
				return false;

			var _data = new Body { coor = _coor, assembly = _assembly };
			return Add(_data, _checkIsland);
		}

		private bool Add(Body _body, bool _checkIsland)
		{
			D.Assert(_body.assembly.availiable);

			mBodies.allowIsland = !_checkIsland;
			var _ret = mBodies.TryAdd(_body.coor, new HexNode<Body>(_body));
			mBodies.allowIsland = false;

			if (_ret)
			{
				mBodyToCoor.Add(_body.assembly, _body.coor);
				MarkNotAvailable(_body.assembly);
			}

			return _ret;
		}

		public bool TryAdd(HexCoor _coor, HexEdge _side, ArmAssembly _assembly)
		{
			if (!BeforeAdd(_assembly))
				return false;

			HexNode<Body> _body;
			if (!mBodies.TryGet(_coor, out _body))
				return false;

			var _data = new Arm { bodyCoor = _coor, side = _side, assembly = _assembly };
			return Add(_data);
		}

		private bool Add(Arm _arm)
		{
			D.Assert(_arm.assembly.availiable);
			var _ret = mArms.TryAdd(_arm.armCoor, new HexNode<Arm>(_arm));
			if (_ret)
			{
				MarkNotAvailable(_arm.assembly);
				mArmToCoor.Add(_arm.assembly, _arm.armCoor);
			}
			return _ret;
		}

		public bool RemoveBody(AssemblyID _id)
		{
			HexCoor _coor;
			if (!mBodyToCoor.TryGet(_id, out _coor))
				return false;
			var _body = mBodies.GetAndRemove(_coor);
			MarkAvailable(_body.assembly);
			return mBodyToCoor.TryRemove(_id);
		}

		public bool RemoveArm(AssemblyID _id)
		{
			HexCoor _coor;
			if (!mArmToCoor.TryGet(_id, out _coor))
				return false;
			var _arm = mArms.GetAndRemove(_coor);
			MarkAvailable(_arm.assembly);
			return mArmToCoor.TryRemove(_id);
		}

		public IEnumerable<Body> GetBodyEnum()
		{
			return mBodies.Select(_node => _node.Value.data);
		}

		public IEnumerable<Arm> GetArmEnum()
		{
			return mArms.Select(_node => _node.Value.data);
		}

		public static NeoBlueprint Load(AssemblyStorage _storage, JObject _data)
		{
			var _ret = new NeoBlueprint();
			var _bodies = _data["bodies"].ToObject<List<Body>>();
			var _arms = _data["arms"].ToObject<List<Arm>>();

			foreach (var _body in _bodies)
			{
				BodyAssembly _assembly;
				if (!_storage.TryGet(_body.assemblyID, out _assembly))
					continue;

				var _tmp = _body;
				_tmp.assembly = _assembly;
				_ret.Add(_tmp, false);
			}

			foreach (var _arm in _arms)
			{
				ArmAssembly _assembly;
				if (!_storage.TryGet(_arm.assemblyID, out _assembly))
					continue;

				var _tmp = _arm;
				_tmp.assembly = _assembly;
				_ret.Add(_tmp);
			}

			return _ret;
		}

		public JObject Save()
		{
			var _ret = new JObject();
			_ret["bodies"] = JToken.FromObject(mBodies.Select(x => x.Value.data).ToList());
			_ret["arms"] = JToken.FromObject(mArms.Select(x => x.Value.data).ToList());
			return _ret;
		}
	}
}