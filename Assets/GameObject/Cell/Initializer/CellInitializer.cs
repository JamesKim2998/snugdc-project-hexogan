using UnityEngine;

namespace HX
{
	public class CellInitializer : MonoBehaviour
	{
		public Cell cellPrf;

		private void Start()
		{
			Setup();
		}

		public void Setup()
		{
			CellDatabase.cellPrf = cellPrf;
		}
	}
}