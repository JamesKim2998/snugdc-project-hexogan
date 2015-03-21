using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(Projectile))]
	public class ProjectileDecoratorDeadzone : MonoBehaviour
	{
		private int mDeadzoneColliderID = 0;

		public Collider2D deadzone
		{
			set { mDeadzoneColliderID = value != null ? value.GetInstanceID() : 0; }
		}

		void Start()
		{
			if (mDeadzoneColliderID == 0)
			{
				Debug.LogWarning("deadzone does not have colliderID. destroy self.");
				Destroy(this);
			}

			GetComponent<Projectile>().Deactivate();
		}

		void OnTriggerExit2D(Collider2D _other)
		{
			if (_other.GetInstanceID() == mDeadzoneColliderID)
			{
				GetComponent<Projectile>().Activate();
				Destroy(this);
			}
		}

	}
}