using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(Explosion))]
	public class ExplosionEditor : Editor<Explosion>
	{
		public void OnSceneGUI()
		{
			if (!target.enabled) return;

			Handles.color = new Color(1, 0, 0, 0.1f);
			Handles.DrawSolidArc(
				target.transform.position,
				Vector3.forward,
				target.transform.right,
				360,
				target.radius);

			Handles.color = new Color(0.5f, 0.5f, 0, 0.5f);
			Handles.DrawWireArc(
				target.transform.position,
				Vector3.forward,
				target.transform.right,
				360,
				target.impulseRadius);
		}

	}
}