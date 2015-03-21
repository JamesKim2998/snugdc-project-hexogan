using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(NeoBody))]
	public class NeoBodyCore : MonoBehaviour
	{
		[SerializeField, UsedImplicitly]
		private NeoBody mBody;
		public NeoBody body { get { return mBody; } }
	}
}