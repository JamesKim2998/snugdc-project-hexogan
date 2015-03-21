using Gem;
using UnityEngine;
using UnityEditor;

namespace HX
{
	[CustomEditor(typeof(ArcTargeter))]
	public class ArcGuiderEditor : Editor<ArcTargeter>
	{
		public void OnSceneGUI()
		{
			if (!target.enabled) return;

			var _radius = target.radius;
			var _color = Color.gray;
			_color.a = 0.1f;

			Handles.color = _color;

			Handles.DrawSolidArc(
				target.transform.position,
				Vector3.forward,
				target.transform.right,
				target.range,
				_radius);

			Handles.DrawSolidArc(
				target.transform.position,
				Vector3.forward,
				target.transform.right,
				-target.range,
				_radius);
		}
	}
}