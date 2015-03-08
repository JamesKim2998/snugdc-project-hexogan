using UnityEngine;

namespace HX.UI
{
	public class ConstructorEntityController : MonoBehaviour
	{
		private ConstructorEntityView m_View;
		private IConstructorItem m_Item;

		public void SetView(ConstructorEntityView _view)
		{
			if (m_View) m_View.postPressDown -= ListenPressDown;

			m_View = _view;

			if (m_View)
			{
				m_View.postPressDown += ListenPressDown;
				if (m_Item != null) m_View.Setup(m_Item);
			}
		}

		public void SetItem(IConstructorItem _item)
		{
			m_Item = _item;
			if (m_View) m_View.Setup(m_Item);
		}

		void ListenPressDown()
		{
			if (m_Item == null) return;
			var _dnd = m_Item.MakeDragAndDrop();
			_dnd.ForcedStick();
			_dnd.offset = Vector2.zero;
		}
	}
}