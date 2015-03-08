using UnityEngine;
using System.Collections;

namespace HX.UI
{
	public class NeoConstructorEntityTest : MonoBehaviour
	{
		public ConstructorEntityView view;
		public NeoMechanicData data;

		private void Start()
		{
			var _ctrl = view.gameObject.AddComponent<ConstructorEntityController>();
			_ctrl.SetView(view);
			_ctrl.SetItem(new NeoConstructorItem(data));
		}
	}
}