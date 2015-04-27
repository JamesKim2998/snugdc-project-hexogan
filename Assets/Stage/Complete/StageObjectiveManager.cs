using System.Collections.Generic;
using Gem;
using Newtonsoft.Json.Linq;

namespace HX
{
	public class StageObjectiveManager
	{
		public bool isStarted { get; private set; }
		public bool isCompleted { get; private set; }

		private readonly Dictionary<string, StageObjective> mObjectives = new Dictionary<string, StageObjective>();
		
		public void Setup(JObject _def)
		{
			foreach (var kv in _def)
				Add(kv.Key, (JObject)kv.Value);
		}

		private void Add(string _key, JObject _def)
		{
			D.Assert(!isStarted && !isCompleted);

			var _complete = StageObjective.Make(_def);

			if (_complete == null)
				return;

			mObjectives[_key] = _complete;
		}
		
		public void Start()
		{
			if (isStarted || isCompleted)
			{
				L.E("cannot start.");
				return;
			}

			isStarted = true;

			foreach (var kv in mObjectives)
			{
				var _objective = kv.Value;
				var _trigger = _objective.trigger;
				_trigger.onTrigger = delegate { OnTrigger(_objective); };
				_trigger.Register();
			}
		}

		public void Stop()
		{
			if (!isStarted)
			{
				L.E("not started.");
				return;
			}

			isStarted = false;

			foreach (var kv in mObjectives)
			{
				var _trigger = kv.Value.trigger;
				_trigger.Unregister();
				_trigger.onTrigger = null;
			}
		}

		private void OnTrigger(StageObjective _complete)
		{
			D.Assert(isStarted && !isCompleted);
			var _condition = _complete.condition;
			if (!_condition.Check()) return;
			isCompleted = true;
			Stop();
		}
	}
}
