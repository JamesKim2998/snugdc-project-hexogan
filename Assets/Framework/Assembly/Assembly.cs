using System;
using Gem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HX
{
	[Serializable]
	public enum AssemblyID {}

	public class Assembly
	{
		public static AssemblyID allocID { get; private set; }

		public readonly AssemblyID id;

		public NeoMechanicType mechanicType { get { return staticData.mechanicType; } }
		public readonly NeoMechanicData staticData;

		public bool availiable = true;

		protected Assembly(NeoMechanicData _staticData)
		{
			id = AllocateID();
			staticData = _staticData;
		}

		protected Assembly(AssemblyID _id, NeoMechanicData _staticData)
		{
			id = _id;
			staticData = _staticData;
		}

		public virtual void Read(JObject _data) { }

		public virtual void Write(JObject _data)
		{
			_data["mechanicType"] = mechanicType.ToString();
		}

		public static AssemblyID AllocateID()
		{
			return allocID++;
		}

		public static void ResetAllocateID(AssemblyID _val)
		{
			allocID = _val;
		}

		public static implicit operator AssemblyID(Assembly _this)
		{
			return _this.id;
		}
	}

	public class BodyAssembly : Assembly
	{
		public NeoBodyType type { get { return staticData.key; } }

		[JsonIgnore]
		public new NeoBodyData staticData { get { return (NeoBodyData)base.staticData; } }

		public BodyAssembly(NeoMechanicData _staticData)
			: base(_staticData)
		{ }

		public BodyAssembly(AssemblyID _id, NeoMechanicData _staticData)
			: base(_id, _staticData)
		{ }

		public override void Read(JObject _data)
		{
			base.Read(_data);
		}

		public override void Write(JObject _data)
		{
			base.Write(_data);
			_data["type"] = staticData.key.ToString();
		}
	}

	public class ArmAssembly : Assembly
	{
		public NeoArmType type { get { return staticData.key; } }

		[JsonIgnore]
		public new NeoArmData staticData { get { return (NeoArmData)base.staticData; } }

		public ArmAssembly(NeoArmData _staticData)
			: base(_staticData)
		{ }

		public ArmAssembly(AssemblyID _id, NeoArmData _staticData)
			: base(_id, _staticData)
		{ }

		public override void Read(JObject _data)
		{
			base.Read(_data);
		}

		public override void Write(JObject _data)
		{
			base.Write(_data);
			_data["type"] = staticData.key.ToString();
		}
	}

	public static class AssemblyHelper
	{
		public static AssemblyID MakeID(string _val)
		{
			return EnumHelper.ParseAsIntOrDefault<AssemblyID>(_val);
		}
	}
}
