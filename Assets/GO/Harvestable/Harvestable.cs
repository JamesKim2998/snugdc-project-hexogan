using Gem;
using UnityEngine;

namespace HX
{
	public abstract class Harvestable : MonoBehaviour
	{
		public readonly HarvestableType type;

		public bool isAggroed { get; private set; }
		public bool isHarvested { get; private set; }

		public GameObject aggroTarget { get; private set; }

		protected Harvestable(HarvestableType _type)
		{
			type = _type;
		}

		public virtual bool IsAggroable(GameObject _target)
		{
			return true;
		}

		public bool TryEnterAggro(GameObject _target)
		{
			if (isAggroed)
			{
				L.E("aggro again.");
				return false;
			}

			if (isHarvested)
			{
				L.E("already harvested.");
				return false;
			}

			var _ret = IsAggroable(_target);
			if (_ret) EnterAggro(_target);
			return _ret;
		}

		private void EnterAggro(GameObject _target)
		{
			isAggroed = true;
			aggroTarget = _target;
			DoEnterAggro();
		}

		protected virtual void DoEnterAggro() { }

		public bool TryExitAggro()
		{
			if (!isAggroed)
			{
				L.E("not aggroed.");
				return false;
			}

			ExitAggro();
			return true;
		}

		private void ExitAggro()
		{
			DoExitAggro();
			isAggroed = false;
			aggroTarget = null;
		}

		protected virtual void DoExitAggro() { }

		public void BeHarvested()
		{
			if (isHarvested)
			{
				L.E("already harvested.");
				return;
			}

			isHarvested = true;
			DoBeHarvested();
		}

		protected abstract void DoBeHarvested();
	}
}