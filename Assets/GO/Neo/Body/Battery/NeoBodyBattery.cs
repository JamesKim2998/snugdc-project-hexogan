using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(NeoBody))]
	public class NeoBodyBattery : MonoBehaviour
	{
		public float energyLeft { get; private set; }
		public NeoBodyBatteryData data { get; private set; }

		void Start()
		{
			GetComponent<NeoMechanic>().onSetupData += OnSetupData;
		}

		void OnDestroy()
		{
			GetComponent<NeoMechanic>().onSetupData -= OnSetupData;
		}

		private void OnSetupData(NeoMechanic _mechanic)
		{
			if (data)
			{
				Debug.LogWarning("Trying to setup data again. Ignore.");
				return;
			}

			data = _mechanic.data.GetProperty<NeoBodyBatteryData>();
			energyLeft = data.capacity;
		}
	}
}