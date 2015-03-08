using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(DatabaseInitializer))]
	public class DatabaseInitializerEditor : Editor
	{
		private DatabaseInitializer m_Target;

		void OnEnable()
		{
			m_Target = (DatabaseInitializer)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Build"))
				m_Target.Build();
		}
	}
}