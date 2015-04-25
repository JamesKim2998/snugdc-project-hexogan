using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof (Harvestable))]
	public class HarvestableEditor : Editor<Harvestable>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			if (GUILayout.Button("be harvested"))
				target.BeHarvested();
		}
	}

	[CustomEditor(typeof(AssemblyPieceHarvestable))]
	public class AssemblyPieceHarvestableEditor : HarvestableEditor { }
}