using System;
using Gem;

namespace HX
{
	public class NeoEnergyController
	{
		private int mCapacity;

		public int capacity
		{
			get { return mCapacity; }
			private set
			{
				D.Assert(value >= 0);
				if (capacity == value) return;
				mCapacityOld = mCapacity;
				mCapacity = value;
			}
		}

		private int mGeneration;
		public int generation
		{
			get { return mGeneration; }
			set { D.Assert(value >= 0); mGeneration = value; }
		}

		private int mConsumption;
		public int consumption
		{
			get { return mConsumption; }
			set { D.Assert(value >= 0); mConsumption = value; }
		}

		public float available { get; private set; }

		public bool isCapacityDirty { get { return mCapacityOld != null; } }
		private int? mCapacityOld;
		public static Action<int, int> onCapacityChanged;

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
				DoConsume(-_netGeneration);

			if (isCapacityDirty)
			{
				onCapacityChanged.CheckAndCall(capacity, mCapacityOld.Value);
				mCapacityOld = null;
			}
		}

		public void AddCapacityAndCharge(int _val)
		{
			capacity += _val;
			Charge(_val);
		}

		public void RemoveCapacityAndCut(int _val)
		{
			var _oldCapacity = capacity;
			capacity -= _val;
			available *= capacity / (float)_oldCapacity;
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
			D.Assert(_val >= 0);
			available -= _val;
			if (available < 0) available = 0;
		}
	}
}