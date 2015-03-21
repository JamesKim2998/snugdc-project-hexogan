using System.Collections.Generic;
using UnityEngine;

namespace HX
{
	public class NeoArmEmitters : MonoBehaviour
	{
		public int neoID;
		public NeoRigidbody mNeoBody;
		public NeoRigidbody neoBody
		{
			get { return mNeoBody; }
			set
			{
				if (mNeoBody == value) return;
				mNeoBody = value;
				foreach (var _emitter in mEmitters)
					_emitter.emitter.ownerBody = neoBody;
			}
		}

		private readonly HashSet<NeoArmEmitter> mEmitters = new HashSet<NeoArmEmitter>();


		public void Add(NeoArmEmitter _arm)
		{
			_arm.emitter.owner = neoID;
			_arm.emitter.ownerBody = neoBody;
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