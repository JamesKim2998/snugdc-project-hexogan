using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NeoBody))]
public class NeoBodyBattery : MonoBehaviour
{
	public float energyLeft { get; private set; }
	public NeoBodyBatteryData data { get; private set; }

	void Start()
	{
		GetComponent<NeoMechanic>().postSetupData += ListenSetupData;
	}

	void OnDestroy()
	{
		GetComponent<NeoMechanic>().postSetupData -= ListenSetupData;
	}

	private void ListenSetupData(NeoMechanic _mechanic)
	{
		if (data)
		{
			Debug.LogWarning("Trying to setup data again. Ignore.");
			return;
		}

		data = _mechanic.data.GetComponent<NeoBodyBatteryData>();
		energyLeft = data.capacity;
	}
}
