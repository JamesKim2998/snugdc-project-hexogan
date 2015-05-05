using Gem;
using UnityEditor;
using UnityEngine;

namespace HX.Stage
{
	[CustomEditor(typeof(EnergyBar))]
	public class EnergyBarEditor : Editor<EnergyBar>
	{
		private int mMax;
		private float mValue;

		protected override void OnEnable()
		{
			mMax = target.max;
			mValue = target.value;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			mMax = EditorGUILayout.IntField("max", mMax);
			mValue = EditorGUILayout.FloatField("value", mValue);

			if (GUILayout.Button("commit"))
			{
				target.max  = mMax;
				target.value = mValue;
			}
		}
	}
}