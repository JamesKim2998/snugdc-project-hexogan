using Gem;
using UnityEngine;

namespace HX.UI
{
	public class GarageController : MonoBehaviour
	{
		public bool isPossessed { get { return mNeoController != null; } }

		private Neo mNeo;

		[SerializeField] private GameObject mWorldView;

		private NeoController mNeoController;

		
		void Start()
		{
			ReplaceNeo();
		}

		void ReplaceNeo()
		{
			if (mNeo)
				Destroy(mNeo.gameObject);

			mNeo = NeoUtil.InstantiateNeo();
			mNeo.transform.SetParent(mWorldView.transform, false);
			mNeo.mechanics.Build(UserManager.neoStructure);

			var _boundingRect = mNeo.mechanics.boundingRect;
			mNeo.transform.localPosition = -_boundingRect.center;
		}

		void Possess(bool _val)
		{
			if (isPossessed == _val)
			{
				L.W("try again.");
				return;
			}

			if (_val)
			{
				mNeoController = gameObject.AddComponent<NeoController>();
				mNeoController.neo = mNeo;
			}
			else
			{
				Destroy(mNeoController);
				mNeoController = null;
				ReplaceNeo();
			}
		}

		public void OnClickPossessButton()
		{
			Possess(!isPossessed);
		}
	}
}
