using Gem;
using UnityEngine;

namespace HX.UI
{
	public class LobbyController : MonoBehaviour
	{
		[SerializeField] private AnatomyData mAnatomyData;
		[SerializeField] private AnatomyView mAnatomy;

		[SerializeField] private UIButton mGameStartButton;

		private WorldTransitionData mTransition;

		void Start()
		{
			mTransition.scene = "world";

			// hardcode
			mAnatomy.Setup(mAnatomyData);
			mAnatomy.onSelectVertex += OnSelectAnatomyVertex;

			mGameStartButton.onClick.Add(new EventDelegate(OnClickStartButton));
		}

		void OnClickStartButton()
		{
			if (string.IsNullOrEmpty(mTransition.tmxPath))
			{
				L.E("tmx is not specified.");
				return;
			}

			TransitionManager.StartWorld(mTransition);
		}

		void OnSelectAnatomyVertex(AnatomyVertexView v)
		{
			mTransition.tmxPath = mAnatomy.data.tmxDir + v.data.tmx;
		}
	}
}