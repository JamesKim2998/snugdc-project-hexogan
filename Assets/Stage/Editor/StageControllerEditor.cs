using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(StageController))]
	public class StageControllerEditor : Editor<StageController>
	{
		private string mStageDir = "TMX/HUMAN";
		private string mStageName = "HUMAN_FOOT";

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			mStageDir = EditorGUILayout.TextField("dir", mStageDir);
			mStageName = EditorGUILayout.TextField("name", mStageName);

			if (GUILayout.Button("setup"))
			{
				var _data = new StageTransitionData();
				_data.dir = new Directory(mStageDir);
				_data.name = mStageName;
				target.Setup(_data);
			}

			if (GUILayout.Button("commit"))
				target.result.CommitAndSave();	
		}
	}
}