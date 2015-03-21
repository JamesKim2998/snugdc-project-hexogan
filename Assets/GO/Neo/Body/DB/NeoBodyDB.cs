using Gem;

namespace HX
{
	public class NeoBodyDB : DB<NeoBodyType, NeoBodyData>
	{
		public static NeoBodyDB g;

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