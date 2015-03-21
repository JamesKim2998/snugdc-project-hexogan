using UnityEngine;

namespace HX
{
	public class EmitterDecoratorRecoil : MonoBehaviour
	{
		private Emitter mEmitter;

		public float recoil = 0;
		public Vector2 recoilModifier = Vector2.one;


		void Start()
		{
			mEmitter = GetComponent<Emitter>();
			mEmitter.onShoot += OnShoot;
		}

		void OnDestroy()
		{
			if (mEmitter) mEmitter.onShoot -= OnShoot;
		}

		void OnShoot(Emitter _emitter, Projectile _projectile)
		{
			if (!_emitter.ownerBody)
			{
				Debug.LogWarning("Emitter doesn't have owner body. Ignore.");
				return;
			}

			if (Mathf.Approximately(recoil, 0))
				return;

			var _recoil = recoil * -transform.right;
			_recoil.x *= recoilModifier.x;
			_recoil.y *= recoilModifier.y;

			_emitter.ownerBody.AddForceAtPosition(
				_recoil, transform.position,
				ForceMode2D.Impulse);
		}
	}
}