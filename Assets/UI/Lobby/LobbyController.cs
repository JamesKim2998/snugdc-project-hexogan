using Gem;
using UnityEngine;

namespace HX.UI
{
	public class LobbyController : MonoBehaviour
	{
		[SerializeField]
		private AnatomyData mAnatomyData;
		[SerializeField]
		private AnatomyView mAnatomy;

		[SerializeField]
		private UIButton mGameStartButton;
		 
		[SerializeField]
		private Neo mNeo;

		[SerializeField]
		private NeoButton mNeoButton;

		private StageTransitionData mTransition;

		void Start()
		{
			mAnatomy.Setup(mAnatomyData);
			mAnatomy.onSelectVertex += OnSelectAnatomyVertex;

			mGameStartButton.onClick.Add(new EventDelegate(OnClickStartButton));

			mNeo.mechanics.Build(AssemblyManager.blueprint);
			mNeoButton.onClick += OnClickNeoButton;

			mTransition.scene = "world";
		}

		void OnClickStartButton()
		{
			if (string.IsNullOrEmpty(mTransition.tmxPath))
			{
				L.E("tmx is not specified.");
				return;
			}

			TransitionManager.StartStage(mTransition);
		}

		void OnSelectAnatomyVertex(AnatomyVertexView v)
		{
			mTransition.dir = mAnatomy.data.stageDir;
			mTransition.name = v.data.key.ToString();
		}

		static void OnClickNeoButton()
		{
			TransitionManager.StartGarage();
		}
	}
}
