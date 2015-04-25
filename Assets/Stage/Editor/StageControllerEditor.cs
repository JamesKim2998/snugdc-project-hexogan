using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(StageController))]
	public class StageControllerEditor : Editor<StageController>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			if (GUILayout.Button("commit"))
				target.result.Commit();
		}
	}
}