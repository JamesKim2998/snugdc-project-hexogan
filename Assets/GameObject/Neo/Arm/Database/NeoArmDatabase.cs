namespace HX
{
	public class NeoArmDatabase : Database<NeoArmType, NeoArmData>
	{
		public static NeoArmDatabase g;

		public override void Build()
		{
			base.Build();
			foreach (var _data in this)
				_data.Build();
		}
	}
}