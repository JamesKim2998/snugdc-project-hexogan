using UnityEngine;

namespace HX.Stage
{
	public class ResultPopup : MonoBehaviour
	{
		public void OnClickGoBackLobby()
		{
			TransitionManager.StartLobby();
		}
	}
}