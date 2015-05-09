using System.Collections.Generic;
using Gem;

namespace HX.Stage
{
	public static class LostAssemblyManager
	{
		private static readonly List<NeoMechanic> sList = new List<NeoMechanic>();

		public static void Setup()
		{
			Clear();
			StageEvents.onMechanicLost += Add;
			StageEvents.onMechanicDecay += OnMechanicDecay;
		}

		public static void Purge()
		{
			StageEvents.onMechanicLost -= Add;
			StageEvents.onMechanicDecay -= OnMechanicDecay;
		}

		public static void Add(NeoMechanic _mechanic)
		{
			D.Assert(!sList.Contains(_mechanic));
			sList.Add(_mechanic);
		}

		public static bool Remove(NeoMechanic  _mechanic)
		{
			var _found = sList.Remove(_mechanic);
			D.Assert(_found);
			return _found;
		}

		public static void Clear()
		{
			sList.Clear();
		}

		private static void OnMechanicDecay(NeoMechanic _mechanic)
		{
			Remove(_mechanic);
		}
	}
}