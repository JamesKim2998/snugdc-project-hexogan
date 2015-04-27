using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(StageController))]
	public class StageControllerEditor : Editor<StageController>
	{
		private string mSetupPath = "TMX/HUMAN/HUMAN_FOOT.json";

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			mSetupPath = EditorGUILayout.TextField("setup", mSetupPath);
			if (GUILayout.Button("setup"))
				target.Setup(new Path(mSetupPath));

			if (GUILayout.Button("start"))
				target.StartStage();

			if (GUILayout.Button("commit"))
				target.result.CommitAndSave();	
		}
	}
}