using Gem.Expression;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HX
{
	public enum ConditionType
	{
		STAGE_REACH = 1,
	}

}

namespace HX.Stage
{
	public sealed class ConditionStageReach : Condition
	{
		private readonly Expression mDestination;
		private readonly float mRadiusSQ;

		public ConditionStageReach(JObject _def)
			: base(ConditionType.STAGE_REACH)
		{
			mDestination = Solver.g.Build(_def["position"]);

			var _radius = (float)_def["radius"];
			mRadiusSQ = _radius * _radius;
		}

		public override bool Check()
		{
			var _neo = StageController.g.neo;
			if (!_neo) return false;

			var _neoPosition = (Vector2)_neo.transform.position;
			var _destination = (Vector2)mDestination();

			var _distanceSQ = (_neoPosition - _destination).sqrMagnitude;
			return _distanceSQ < mRadiusSQ;
		}
	}
}
