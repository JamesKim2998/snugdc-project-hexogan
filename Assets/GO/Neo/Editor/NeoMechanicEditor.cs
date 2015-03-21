using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(NeoMechanic))]
	public class NeoMechanicEditor : Editor<NeoMechanic>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (PrefabHelper.IsPrefab(target))
				return;

			GUILayout.Label(
				"CohesionLeft " + target.cohesionLeft
				+ ", DurabilityLeft " + target.durabilityLeft);

			if (target.parent)
			{
				if (GUILayout.Button("Detach"))
					target.parent.Remove(target);
			}
			else
			{
				if (!target.gameObject.GetComponent<Gem.DragAndDrop>())
				{
					if (GUILayout.Button("Drag and drop"))
					{
						target.gameObject.AddComponent<Gem.DragAndDrop>();
						NeoMechanicHelper.AddDragAndDrop(target);
					}
				}
			}
		}

		public void OnSceneGUI()
		{
			base.OnInspectorGUI();

			if (target.parent == null)
			{
				Handles.color = Color.red;

				Handles.DrawSolidArc(
					target.transform.TransformPoint(target.com),
					Vector3.forward, Vector3.right,
					360, 0.02f);
			}
		}
	}

	[CustomEditor(typeof(NeoArm))]
	public class NeoArmEditor : NeoMechanicEditor { }

	[CustomEditor(typeof(NeoBody))]
	public class NeoBodyEditor : NeoMechanicEditor { }
}