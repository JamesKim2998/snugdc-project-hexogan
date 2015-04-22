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
		public static AssemblyID sAllocID;

		public readonly AssemblyID id;
		public readonly NeoMechanicData staticData;

		public bool availiable = true;

		protected Assembly(NeoMechanicData _staticData)
		{
			id = sAllocID++;
			staticData = _staticData;
		}

		protected Assembly(AssemblyID _id, NeoMechanicData _staticData)
		{
			id = _id;
			staticData = _staticData;
		}

		public virtual void Read(JObject _data)
		{
			
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
	}

	public static class AssemblyHelper
	{
		public static AssemblyID MakeID(string _val)
		{
			return EnumHelper.ParseAsIntOrDefault<AssemblyID>(_val);
		}
	}
}
