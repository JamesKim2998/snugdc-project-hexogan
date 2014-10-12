using UnityEngine;
using System.Collections;

namespace neo.ui.constructor
{
	public class Constructor : MonoBehaviour
	{
		public UIGrid bodyGrid;
		public UIGrid armGrid;

		public EntityView bodyViewPrf;
		public EntityView armViewPrf;

		public Hammer hammerPrf;

		void Start()
		{
			foreach (var _data in NeoBodyDatabase.shared)
				AddToGrid(bodyGrid, bodyViewPrf, _data);
			foreach (var _data in NeoArmDatabase.shared)
				AddToGrid(armGrid, armViewPrf, _data);

			bodyGrid.Reposition();
			armGrid.Reposition();
		}

		static void AddToGrid(UIGrid _grid, EntityView _viewPrf, NeoMechanicData _data)
		{
			var _view = (GameObject)Instantiate(_viewPrf.gameObject);
			var _ctrl = _view.AddComponent<EntityController>();
			_ctrl.SetView(_view.GetComponent<EntityView>());
			_ctrl.SetData(_data);
			TransformHelper.SetParentWithoutScale(_ctrl, _grid);
		}

		public void PickHammer()
		{
			Instantiate(hammerPrf.gameObject);
		}
	}

}