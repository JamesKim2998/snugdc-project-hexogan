using Gem;
using UnityEngine;

namespace HX.UI.Garage
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

			var _item = mData.materialPrf.Instantiate();
			_item.transform.SetParent(transform, false);
			_item.transform.localPosition = mItemPosition;
		}

		public MaterialDND MakeDragAndDrop()
		{
			var _material = mData.materialPrf.Instantiate();
			_material.transform.SetParent(GarageController.g.uiRoot.transform, false);

			MaterialDND _dnd = null;

			switch (mData.mechanicType)
			{
				case NeoMechanicType.BODY:
					var _bodyDND = _material.AddComponent<MaterialBodyDND>();
					_bodyDND.type = ((NeoBodyData)mData).key;
					_dnd = _bodyDND;
					break;
				case NeoMechanicType.ARM:
					var _armDND = _material.AddComponent<MaterialArmDND>();
					_armDND.type = ((NeoArmData)mData).key;
					_dnd = _armDND;
					break;
			}

			return _dnd;
		}

		public void OnPressCell()
		{
			if (mData == null) return;
			var _dnd = MakeDragAndDrop();
			_dnd.ForcedStick();
			_dnd.offset = Vector2.zero;
		}
	}
}