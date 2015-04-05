using System;
using Gem;
using UnityEngine;

namespace HX.UI
{
	public class AnatomyVertexView : MonoBehaviour
	{
		public OrganData data { get; private set; }

		[SerializeField]
		private UISprite mSprite;

// 		public new Animation animation;

		public Action<AnatomyVertexView> onClick;

		public void Setup(OrganData _data)
		{
			data = _data;
			transform.localPosition = _data.vertexPosition;
		}

		public void SetLock(bool _val)
		{
			mSprite.color = _val ? Color.green : Color.red;
		}

		public void Select(bool _val)
		{
			L.W("select");
		}

		public void Highlight(bool _val)
		{
			L.W("highlight");
		}

		private void OnClick()
		{
			D.Assert(data != null);
			onClick.CheckAndCall(this);
		}
	}
}