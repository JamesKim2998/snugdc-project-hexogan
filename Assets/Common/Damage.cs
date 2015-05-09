namespace HX
{
	[System.Serializable]
	public class Damage
	{
		public static readonly Damage MAX = new Damage(float.MaxValue);

		public float value;

		public Damage(float _value)
		{
			value = _value;
		}

		public static implicit operator float(Damage _this)
		{
			return _this.value;
		}
	}
}