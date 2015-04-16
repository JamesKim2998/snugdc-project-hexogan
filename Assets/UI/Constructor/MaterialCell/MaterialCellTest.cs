﻿#if UNITY_EDITOR

using UnityEngine;

namespace HX.UI.Constructor
{
	public class NeoMaterialCellTest : MonoBehaviour
	{
		[SerializeField] private MaterialCell mView;
		[SerializeField] private NeoMechanicData mData;

		private void Start()
		{
			mView.SetData(mData);
		}
	}
}

#endif