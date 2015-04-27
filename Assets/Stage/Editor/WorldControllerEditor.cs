using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(WorldController))]
	public class WorldControllerEditor : Editor<WorldController>
	{
		private string mTmxPath = "TMX/HUMAN/HUMAN_FOOT.tmx";

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			mTmxPath = EditorGUILayout.TextField("tmx path", mTmxPath);
			if (!string.IsNullOrEmpty(mTmxPath))
			{
				if (GUILayout.Button("load"))
					target.SetupGrid(new Path(mTmxPath));
			}
		}
	}
}