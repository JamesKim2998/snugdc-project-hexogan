using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(NeoMechanic))]
	public class NeoMechanicEditor : Editor
	{
		private NeoMechanic m_This;

		void OnEnable()
		{
			m_This = (NeoMechanic)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (PrefabHelper.IsPrefab(m_This))
				return;

			GUILayout.Label(
				"CohesionLeft " + m_This.cohesionLeft
				+ ", DurabilityLeft " + m_This.durabilityLeft);

			if (m_This.parent)
			{
				if (GUILayout.Button("Detach"))
					m_This.parent.Remove(m_This);
			}
			else
			{
				if (!m_This.gameObject.GetComponent<Gem.DragAndDrop>())
				{
					if (GUILayout.Button("Drag and drop"))
					{
						m_This.gameObject.AddComponent<Gem.DragAndDrop>();
						NeoMechanicHelper.AddDragAndDrop(m_This);
					}
				}
			}
		}

		public void OnSceneGUI()
		{
			base.OnInspectorGUI();

			if (!m_This.parent)
			{
				Handles.color = Color.red;

				Handles.DrawSolidArc(
					m_This.transform.TransformPoint(m_This.com),
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