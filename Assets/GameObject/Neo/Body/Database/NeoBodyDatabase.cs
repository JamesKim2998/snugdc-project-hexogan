namespace HX
{
	public class NeoBodyDatabase : Database<NeoBodyType, NeoBodyData>
	{
		public static NeoBodyDatabase g;

		public override void Build()
		{
			base.Build();
			foreach (var _data in this)
				_data.Build();
		}
	}
}