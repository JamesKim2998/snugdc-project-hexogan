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
			public BodyAssembly assembly;
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
			public ArmAssembly assembly;
		}

		private readonly HexGraph<Body> mBodies = new HexGraph<Body>();
		private readonly HexGraph<Arm> mArms = new HexGraph<Arm>();

		public NeoBlueprint()
		{
			mBodies.allowIsland = false;
			mArms.allowIsland = true;
		}

		private static bool BeforeAdd(Assembly _own)
		{
			if (!_own.availiable)
			{
				L.E("mechanic is not available.");
				return false;
			}

			return true;
		}

		private static void MarkNotAvailable(Assembly _own)
		{
			_own.availiable = false;
		}

		public bool TryAdd(HexCoor _coor, BodyAssembly _assembly, bool _checkIsland = true)
		{
			if (!BeforeAdd(_assembly))
				return false;

			var _data = new Body { coor = _coor, assembly = _assembly };
			var _node = new HexNode<Body>(_data);

			mBodies.allowIsland = !_checkIsland;
			var _ret = mBodies.TryAdd(_coor, _node);
			mBodies.allowIsland = false;

			if (_ret) MarkNotAvailable(_assembly);
			return _ret;
		}

		private void Add(Body _body)
		{
			D.Assert(_body.assembly.availiable);
			mBodies.allowIsland = true;
			var _ret = mBodies.TryAdd(_body.coor, new HexNode<Body>(_body));
			mBodies.allowIsland = false;
			if (_ret) _body.assembly.availiable = false;
		}

		public bool TryAdd(HexCoor _coor, HexEdge _side, ArmAssembly _assembly)
		{
			if (!BeforeAdd(_assembly))
				return false;

			HexNode<Body> _body;
			if (!mBodies.TryGet(_coor, out _body))
				return false;

			var _data = new Arm { bodyCoor = _coor, side = _side, assembly = _assembly };
			var _node = new HexNode<Arm>(_data);

			var _ret = mArms.TryAdd(_data.armCoor, _node);
			if (_ret) MarkNotAvailable(_assembly);
			return _ret;
		}

		private void Add(Arm _arm)
		{
			D.Assert(_arm.assembly.availiable);
			if (mArms.TryAdd(_arm.armCoor, new HexNode<Arm>(_arm)))
				_arm.assembly.availiable = false;
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
				_ret.Add(_tmp);
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
			_ret["bodies"] = new JObject(mBodies.ToList());
			_ret["arms"] = new JObject(mArms.ToList());
			return _ret;
		}
	}
}