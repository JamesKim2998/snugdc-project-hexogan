﻿using Gem;
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

			TransitionManager.StartWorld(mTransition);
		}

		void OnSelectAnatomyVertex(AnatomyVertexView v)
		{
			mTransition.tmxPath = new Path(mAnatomy.data.tmxDir + v.data.tmx);
		}

		static void OnClickNeoButton()
		{
			TransitionManager.StartGarage();
		}
	}
}
