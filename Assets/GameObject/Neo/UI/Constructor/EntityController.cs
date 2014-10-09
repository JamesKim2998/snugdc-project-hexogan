using UnityEngine;
using System.Collections;

namespace neo.ui.constructor
{
	public class EntityController : MonoBehaviour
	{
		private EntityView m_View;
		private NeoMechanicData m_Data;

		public void SetView(EntityView _view)
		{
			if (m_View) m_View.postPressDown -= ListenPressDown;

			m_View = _view;

			if (m_View)
			{
				m_View.postPressDown += ListenPressDown;
				if (m_Data) m_View.Setup(m_Data);
			}
		}

		public void SetData(NeoMechanicData _data)
		{
			m_Data = _data;
			if (m_View) m_View.Setup(m_Data);
		}

		void ListenPressDown()
		{
			if (! m_Data) return;

			var _mechanic = (GameObject)Instantiate(m_Data.mechanicPrf.gameObject);

			var _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePos.z = 0;
			_mechanic.transform.position = _mousePos;

			var _dnd = NeoMechanicHelper.AddDragAndDrop(_mechanic.GetComponent<NeoMechanic>());
			_dnd.ForcedStick();
		}
	}
}