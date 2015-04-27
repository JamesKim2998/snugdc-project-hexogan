namespace HX
{
	public partial class StageController
	{
		private bool TryComplete()
		{
			if (isCompleted)
				return false;

			if (!objective.isCompleted)
				return false;
			
			Complete();
			return true;
		}

		private void Complete()
		{
			isCompleted = true;
			DetachNeoController();
			result.CommitAndSave();
		}
	}
}
