using Gem;
using UnityEngine;

namespace HX.UI.Constructor
{
	public class MaterialCell : MonoBehaviour
	{
		[SerializeField] public Vector2 mItemPosition;
		[SerializeField] public UILabel mNameLabel;

		private NeoMechanicData mData;

		public void SetData(NeoMechanicData _data)
		{
			mData = _data;

			mNameLabel.text = _data.name;

			// todo
// 			var _item = mData.materialPrf.Instantiate();
// 			_item.transform.SetParent(transform, false);
// 			_item.transform.localPosition = mItemPosition;
		}

		public DragAndDrop MakeDragAndDrop()
		{
			return null;
			// todo
// 			var _material = mData.materialPrf.Instantiate();
// 			var _dnd = NeoMechanicHelper.AddDragAndDrop(_material);
// 			_dnd.destroyIfFailed = true;
// 			return _dnd;
		}

		void OnClick()
		{
			if (mData == null) return;
			var _dnd = MakeDragAndDrop();
			_dnd.ForcedStick();
			_dnd.offset = Vector2.zero;
		}
	}
}