namespace HX
{
	public static class NeoMechanicHelper
	{
		public static NeoMechanicDragAndDrop AddDragAndDrop(NeoMechanic _mechanic)
		{
			if (_mechanic.GetComponent<NeoBody>())
				return _mechanic.gameObject.AddComponent<NeoBodyDragAndDrop>();
			else if (_mechanic.GetComponent<NeoArm>())
				return _mechanic.gameObject.AddComponent<NeoArmDragAndDrop>();
			return null;
		}
	}
}