using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DatabaseInitializer : MonoBehaviour
{
	private bool m_IsInitialized = false;

	public ProjectileDatabase projectile;
	public EmitterDatabase emitter;

	public NeoBodyDatabase neoBody;
	public NeoArmDatabase neoArm;

	public IEnumerable<IDatabase> GetDatabases()
	{
		Initialize();
		yield return projectile;
		yield return emitter;
		yield return neoBody;
		yield return neoArm;
	}

	void Awake()
	{
		Initialize();
		Build();
	}

	void Initialize()
	{
		if (m_IsInitialized) return;
		m_IsInitialized = true;
		ProjectileDatabase.shared = projectile;
		EmitterDatabase.shared = emitter;
		NeoBodyDatabase.shared = neoBody;
		NeoArmDatabase.shared = neoArm;
	}

	public void Build()
	{
		foreach (var _database in GetDatabases())
			_database.Build();
	}
}
