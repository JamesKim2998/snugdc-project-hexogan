using UnityEngine;

[RequireComponent(typeof(Emitter))]
public class SimpleLauncherDef : MonoBehaviour 
{
	public ProjectileType projectile;

	void Start() 
	{
		var _emitter = GetComponent<Emitter>();

#if DEBUG
		if (_emitter == null) 
		{
			Debug.LogError("Emitter is not exists! Ignore.");
			return;
		}
#endif
		
		var _projectilePrf = ProjectileDatabase.shared[projectile].projectilePrf;

		_emitter.doCreateProjectile = delegate { return (GameObject) Instantiate(_projectilePrf); };

	    _emitter.doCreateProjectileServer = delegate { return (GameObject) Instantiate(_projectilePrf); };
	}
	
}
