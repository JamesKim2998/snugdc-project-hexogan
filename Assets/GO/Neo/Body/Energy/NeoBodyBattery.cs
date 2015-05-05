using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(NeoBody))]
	public class NeoBodyBattery : MonoBehaviour
	{
		[SerializeField, UsedImplicitly]
		private NeoBody mBody;
		public NeoBody body { get { return mBody; } }
		public float energyLeft { get; private set; }
		public MechanicPropertyBattery data { get; private set; }

		private void Awake()
		{
			data = body.data.GetProperty<MechanicPropertyBattery>();
			energyLeft = data.capacity;
		}
	}
}