using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(Emitter))]
	public class EmitterEditor : Editor<Emitter>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (PrefabHelper.IsPrefab(target))
				return;

			if (GUILayout.Button("Shoot"))
				target.TryShoot();
		}
	}
}