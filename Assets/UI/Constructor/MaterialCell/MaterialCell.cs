using System;
using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class MaterialCell : MonoBehaviour
	{
		[SerializeField] public Vector2 mItemPosition;
		[SerializeField] public UILabel mCountLabel;

		private NeoMechanicData mData;

		public Func<AssembleCommand, bool> confirmAssemble;

		void Start()
		{
			GarageEvents.onAssemble += OnAssemble;
			GarageEvents.onDisassemble += OnDisassemble;
		}

		void OnDestroy()
		{
			GarageEvents.onAssemble -= OnAssemble;
			GarageEvents.onDisassemble -= OnDisassemble;
		}

		public void SetData(NeoMechanicData _data)
		{
			mData = _data;

			var _item = mData.materialPrf.Instantiate();
			_item.transform.SetParent(transform, false);
			_item.transform.localPosition = mItemPosition;

			RefreshCount();
		}

		private void RefreshCount()
		{
			SetCount(GetAvailable().Count());
		}

		private void SetCount(int _val)
		{
			mCountLabel.text = "x" + _val;
		}

		private IEnumerable<Assembly> GetAvailable()
		{

			var _storage = AssemblyManager.storage;

			switch (mData.mechanicType)
			{
				case NeoMechanicType.BODY:
				{
					var _type = ((NeoBodyData)mData).key;
					return _storage.GetAvailable(_type).Cast<Assembly>();
				}
				case NeoMechanicType.ARM:
				{
					var _type = ((NeoArmData)mData).key;
					return _storage.GetAvailable(_type).Cast<Assembly>();
				}
			}

			D.Assert(false);
			return null;
		}

		private Assembly GetFirstAssembly()
		{
			return GetAvailable().FirstOrDefault();
		}

		private MaterialDND MakeDragAndDrop()
		{
			var _assembly = GetFirstAssembly();
			if (_assembly == null)
			{
				L.E("assembly not found.");
				return null;
			}

			var _material = mData.materialPrf.Instantiate();
			_material.transform.SetParent(GarageController.g.uiRoot.transform, false);

			MaterialDND _dnd;

			switch (mData.mechanicType)
			{
				case NeoMechanicType.BODY:
					_dnd = _material.AddComponent<MaterialBodyDND>();
					break;
				case NeoMechanicType.ARM:
					_dnd = _material.AddComponent<MaterialArmDND>();
					break;
				default:
					D.Assert(false);
					Destroy(_material.gameObject);
					return null;
			}

			_dnd.assembly = _assembly;
			return _dnd;
		}

		public void OnPressCell()
		{
			if (mData == null) return;
			var _dnd = MakeDragAndDrop();
			if (!_dnd) return;
			_dnd.ForcedStick();
			_dnd.offset = Vector2.zero;
		}

		public void OnAssemble(AssembleCommand _command)
		{
			Invoke("RefreshCount", 0);
		}

		private void OnDisassemble(DisassembleCommand _obj)
		{
			Invoke("RefreshCount", 0);
		}
	}
}