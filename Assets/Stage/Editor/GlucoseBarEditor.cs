using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(GlucoseBar))]
	public class GlucoseBarEditor : Editor<GlucoseBar>
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