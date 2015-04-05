#if UNITY_EDITOR

using UnityEngine;

namespace HX.UI
{
	public class AnatomyTest : MonoBehaviour
	{
		[SerializeField] private AnatomyData mData;
		[SerializeField] private AnatomyView mView;

		void Start()
		{
			mView.Setup(mData);
		}
	}
}

#endif