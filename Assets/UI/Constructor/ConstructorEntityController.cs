using UnityEngine;

namespace HX.UI
{
	public class ConstructorEntityController : MonoBehaviour
	{
		private ConstructorEntityView mView;
		private IConstructorItem mItem;

		public void SetView(ConstructorEntityView _view)
		{
			if (mView) mView.onPressDown -= OnPressDown;

			mView = _view;

			if (mView)
			{
				mView.onPressDown += OnPressDown;
				if (mItem != null) mView.Setup(mItem);
			}
		}

		public void SetItem(IConstructorItem _item)
		{
			mItem = _item;
			if (mView) mView.Setup(mItem);
		}

		void OnPressDown()
		{
			if (mItem == null) return;
			var _dnd = mItem.MakeDragAndDrop();
			_dnd.ForcedStick();
			_dnd.offset = Vector2.zero;
		}
	}
}