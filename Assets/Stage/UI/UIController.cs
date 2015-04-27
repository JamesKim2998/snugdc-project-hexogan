using Gem;
using UnityEngine;

namespace HX.Stage
{
	public class UIController : MonoBehaviour
	{
		public static UIController g { get; private set; }

		public UIRoot root;

		public PopupManager popup { get; private set; }

		void Start()
		{
			popup = new PopupManager(root.transform);

			D.Assert(g == null);
			if (g == null)
				g = this;
		}

		void OnDestroy()
		{
			D.Assert(g == this);
			if (g == this)
				g = null;
		}
	}
}
