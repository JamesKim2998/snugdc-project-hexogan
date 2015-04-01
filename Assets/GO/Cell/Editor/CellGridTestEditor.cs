using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(CellGridTest))]
	public class CellGridTestEditor : Editor<CellGridTest>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("reload"))
			{
				target.grid.DestroyAll();
				target.Load();
			}
		}
	}
}