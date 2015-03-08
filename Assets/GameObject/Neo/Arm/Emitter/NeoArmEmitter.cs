using Gem;
using UnityEngine;

public class NeoArmEmitter : MonoBehaviour
{
	public EmitterType emitterType;
	public Animator animator;

	public Emitter emitter { get; private set; }

	void Start()
	{
		emitter = EmitterDatabase.shared[emitterType].Instantiate();
		emitter.transform.SetParentIdentity(transform);
		emitter.transform.localPosition = Vector3.zero;

		animator = GetComponent<Animator>();
	}

	public bool IsShootable()
	{
		return emitter.IsShootable();
	}

	public bool TryShoot()
	{
		if (! IsShootable()) 
			return false;

		Shoot();
		return true;
	}

	private void Shoot()
	{
		emitter.Shoot();
		animator.SetTrigger("shoot");
	}
}
