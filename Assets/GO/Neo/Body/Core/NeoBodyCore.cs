using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(NeoBody))]
	public class NeoBodyCore : MonoBehaviour
	{
		[SerializeField]
		private NeoBody mBody;
		public NeoBody body { get { return mBody; } }
	}
}