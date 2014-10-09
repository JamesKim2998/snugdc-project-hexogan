using UnityEngine;
using System.Collections;

namespace neo.ui.constructor
{

	public class EntityTest : MonoBehaviour
	{
		public EntityView view;
		public NeoMechanicData data;

		private void Start()
		{
			var _ctrl = view.gameObject.AddComponent<EntityController>();
			_ctrl.SetView(view);
			_ctrl.SetData(data);
		}
	}

}