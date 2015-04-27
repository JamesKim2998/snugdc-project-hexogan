using System;
using Gem;

namespace HX
{
	public abstract class Trigger
	{
		public readonly TriggerType type;

		public bool isRegistered { get; private set; }

		public Action<Trigger> onTrigger;

		protected Trigger(TriggerType _type)
		{
			type = _type;
		}

		~Trigger()
		{
			D.Assert(!isRegistered);
			if (isRegistered)
				Unregister();
		}

		public void Register()
		{
			if (isRegistered)
			{
				L.E("already registered.");
				return;
			}

			isRegistered = true;
			DoRegister();
		}

		public void Unregister()
		{
			if (!isRegistered)
			{
				L.E("not even registered.");
				return;
			}

			DoUnregister();
			isRegistered = false;
		}

		protected void DoTrigger()
		{
			D.Assert(isRegistered);
			onTrigger.CheckAndCall(this);
		}

		protected abstract void DoRegister();
		protected abstract void DoUnregister();
	}
}
