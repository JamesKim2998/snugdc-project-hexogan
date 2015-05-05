using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public class NeoArmMotor : MonoBehaviour
	{
		[SerializeField, UsedImplicitly]
		private NeoArm mArm;
		public NeoArm arm { get { return mArm; }}

		public int thrust;
		public int thrustEnergyConsumption;

		public Animator animator;

		public void TurnOn()
		{
			if (animator) animator.SetTrigger("turn_on");
		}

		public void TurnOff()
		{
			if (animator) animator.SetTrigger("turn_off");
		}
	}
}