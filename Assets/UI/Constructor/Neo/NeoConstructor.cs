using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class NeoConstructor : MonoBehaviour
	{
		[SerializeField] private StashGrid mBodyStash;
		[SerializeField] private StashGrid mArmStash;
		[SerializeField] private Hammer mHammerPrf;

		void Start()
		{
			foreach (var _data in NeoBodyDB.g)
				mBodyStash.Add(_data.Value);
			foreach (var _data in NeoArmDB.g)
				mArmStash.Add(_data.Value);

			mBodyStash.Reposition();
			mArmStash.Reposition();
		}

		public void PickHammer()
		{
			var _hammer = mHammerPrf.Instantiate();
			_hammer.transform.SetParent(GarageController.g.uiRoot.transform, false);
		}
	}

}