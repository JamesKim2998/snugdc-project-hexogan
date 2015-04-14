using UnityEngine;

namespace HX.UI
{
	public class LobbyController : MonoBehaviour
	{
		[SerializeField]
		private MainController mMainController;

		[SerializeField]
		private NeoTransitionAnimator mNeoTransitionAnimator;

		void Start()
		{
			mMainController.onNeoButtonClicked += () => mNeoTransitionAnimator.PlayIn();
		}
	}
}