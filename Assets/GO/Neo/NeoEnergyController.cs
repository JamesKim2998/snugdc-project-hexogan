namespace HX
{
	public class NeoEnergyController 
	{
		public int capacity { get; set; }
		public int generation;
		public int consumption;

		public float available { get; private set; }

		public bool isEmpty()
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			return available == 0f;
		}

		public bool isFull()
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			return available == capacity;
		}

		public void Update(float _dt)
		{
			var _netGeneration = (generation - consumption) * _dt;
			if (_netGeneration >= 0)
				Charge(_netGeneration);
			else
				DoConsume(_netGeneration);
		}

		public void AddCapacityAndCharge(int _val)
		{
			capacity += _val;
			Charge(_val);
		}

		public void RemoveCapacityAndConsume(int _val)
		{
			capacity -= _val;
			DoConsume(_val);
		}

		public void Charge(float _val)
		{
			var _result = available + _val;
			available = (_result < capacity) ? _result : capacity;
		}

		public void ChargeFull()
		{
			available = capacity;
		}

		public bool TryConsume(float _val)
		{
			if (available < _val)
				return false;

			DoConsume(_val);
			return true;
		}

		private void DoConsume(float _val)
		{
			available -= _val;
			if (available < 0) available = 0;
		}
	}
}