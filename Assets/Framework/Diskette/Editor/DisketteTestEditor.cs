using Gem;
using HX.UI;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(DisketteTest))]
	public class DisketteTestEditor : Editor<DisketteTest>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;
			
			if (GUILayout.Button("load"))
			{
				if (!DisketteManager.isLoaded)
					DisketteManager.Load(target.filename);
			}

			if (GUILayout.Button("save"))
				DisketteManager.Save();
		}
	}
}