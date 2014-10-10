using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;

public class NeoArmEmitter : MonoBehaviour
{
	public EmitterType emitterType;
	public Animator animator;

	public Emitter emitter { get; private set; }

	void Start()
	{
		emitter = EmitterDatabase.shared[emitterType].Instantiate();
		TransformHelper.SetParentWithoutScale(emitter, this);
		emitter.transform.localPosition = Vector3.zero;

		animator = GetComponent<Animator>();
	}

	public bool IsShootable()
	{
		return emitter.IsShootable();
	}

	public bool TryShoot()
	{
		return emitter.TryShoot();
	}

	private void Shoot()
	{
		emitter.Shoot();
	}
}
