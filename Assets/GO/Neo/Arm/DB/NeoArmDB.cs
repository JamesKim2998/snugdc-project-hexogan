using Gem;

namespace HX
{
	public class NeoArmDB : DB<NeoArmType, NeoArmData>
	{
		public static NeoArmDB g;

#if UNITY_EDITOR
		public override void Build()
		{
			base.Build();
			foreach (var _data in this)
				_data.Value.Build();
		}
#endif
	}
}