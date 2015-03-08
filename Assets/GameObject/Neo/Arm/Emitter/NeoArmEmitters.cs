using System.Collections.Generic;
using UnityEngine;

namespace HX
{
	public class NeoArmEmitters : MonoBehaviour
	{
		public int neoID;
		public NeoRigidbody m_NeoBody;
		public NeoRigidbody neoBody
		{
			get { return m_NeoBody; }
			set
			{
				if (m_NeoBody == value) return;
				m_NeoBody = value;
				foreach (var _emitter in m_Emitters)
					_emitter.emitter.ownerBody = neoBody;
			}
		}

		private readonly HashSet<NeoArmEmitter> m_Emitters = new HashSet<NeoArmEmitter>();


		public void Add(NeoArmEmitter _arm)
		{
			_arm.emitter.owner = neoID;
			_arm.emitter.ownerBody = neoBody;
			m_Emitters.Add(_arm);
		}

		public void Remove(NeoArmEmitter _arm)
		{
			_arm.emitter.owner = 0;
			_arm.emitter.ownerBody = null;
			m_Emitters.Remove(_arm);
		}

		public void Shoot()
		{
			foreach (var _emitter in m_Emitters)
				_emitter.TryShoot();
		}
	}
}