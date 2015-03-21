using System.Collections.Generic;

namespace HX
{
	public class NeoArmEmitters
	{
		private readonly int mInstanceID;
		public readonly NeoRigidbody body;

		private readonly HashSet<NeoArmEmitter> mEmitters = new HashSet<NeoArmEmitter>();

		public NeoArmEmitters(int _instanceID, NeoRigidbody _body)
		{
			mInstanceID = _instanceID;
			body = _body;
		}

		public void Add(NeoArmEmitter _arm)
		{
			_arm.emitter.owner = mInstanceID;
			_arm.emitter.ownerBody = body;
			mEmitters.Add(_arm);
		}

		public void Remove(NeoArmEmitter _arm)
		{
			_arm.emitter.owner = 0;
			_arm.emitter.ownerBody = null;
			mEmitters.Remove(_arm);
		}

		public void Shoot()
		{
			foreach (var _emitter in mEmitters)
				_emitter.TryShoot();
		}
	}
}