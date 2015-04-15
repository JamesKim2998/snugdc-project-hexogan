using System;
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

		private WorldTransitionData mTransition;

		public Action onNeoButtonClicked;

		void Start()
		{
			mAnatomy.Setup(mAnatomyData);
			mAnatomy.onSelectVertex += OnSelectAnatomyVertex;

			mGameStartButton.onClick.Add(new EventDelegate(OnClickStartButton));

			mNeo.mechanics.Build(UserManager.neoStructure);
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

			TransitionManager.StartWorld(mTransition);
		}

		void OnSelectAnatomyVertex(AnatomyVertexView v)
		{
			mTransition.tmxPath = mAnatomy.data.tmxDir + v.data.tmx;
		}

		void OnClickNeoButton()
		{
			onNeoButtonClicked.CheckAndCall();
		}
	}
}
