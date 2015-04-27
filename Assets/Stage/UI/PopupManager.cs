using Gem;
using UnityEngine;

namespace HX.Stage
{
	public class PopupManager
	{
		private readonly Transform mRoot;

		public PopupManager(Transform _root)
		{
			mRoot = _root;
		}

		public ResultPopup OpenResult()
		{
			var _popup = UIDB.g.resultPopup.Instantiate();
			_popup.transform.SetParent(mRoot, false);
			return _popup;
		}
	}
}
