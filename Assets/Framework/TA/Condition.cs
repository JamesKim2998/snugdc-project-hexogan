namespace HX
{
	public abstract class Condition
	{
		public readonly ConditionType type;

		protected Condition(ConditionType _type)
		{
			type = _type;
		}

		public abstract bool Check();
	}
}
