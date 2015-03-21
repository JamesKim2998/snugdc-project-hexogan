using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(TheHexogan))]
	public class TheHexoganEditor : Editor<TheHexogan> 
	{
		public override void OnInspectorGUI () 
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("build"))
				target.Build();
		}
	}
}