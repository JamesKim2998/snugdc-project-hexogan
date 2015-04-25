using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gem;

namespace HX
{
	public class StageResult
	{
		public bool isCommitted { get; private set; }

		private readonly List<Assembly> mAcquiredAssemblies = new List<Assembly>();
		public readonly ReadOnlyCollection<Assembly> acquiredAssemblies;

		public StageResult()
		{
			acquiredAssemblies = new ReadOnlyCollection<Assembly>(mAcquiredAssemblies);
		}

		public void Acquire(AssemblyPiece _piece)
		{
			var _assembly = _piece.assembly;
			if (_assembly == null)
			{
				L.E("piece has no assembly.");
				return;
			}

			mAcquiredAssemblies.Add(_assembly);
			StageEvents.onAcquireAssembly.CheckAndCall(_assembly);
		}

		public void CommitAndSave()
		{
			if (isCommitted)
			{
				L.E("commit again.");
				return;
			}

			isCommitted = true;

			foreach (var _assembly in mAcquiredAssemblies)
				AssemblyManager.storage.SpecifyAndAdd(_assembly);

			DisketteManager.Save();
		}
	}
}