using System.Collections.Generic;
using UnityEngine;

namespace HX
{
	public class DatabaseInitializer : MonoBehaviour
	{
		private bool mIsInitialized;

		public ProjectileDatabase projectile;
		public EmitterDatabase emitter;

		public NeoBodyDatabase neoBody;
		public NeoArmDatabase neoArm;

		public CellPlasmDatabase cellPlasm;
		public CellWallDatabase cellWall;

		public IEnumerable<IDatabase> GetDatabases()
		{
			Initialize();
			yield return projectile;
			yield return emitter;
			yield return neoBody;
			yield return neoArm;
			yield return cellPlasm;
			yield return cellWall;
		}

		void Awake()
		{
			Initialize();
			Build();
		}

		void Initialize()
		{
			if (mIsInitialized) return;
			mIsInitialized = true;
			ProjectileDatabase.g = projectile;
			EmitterDatabase.g = emitter;
			NeoBodyDatabase.g = neoBody;
			NeoArmDatabase.g = neoArm;
			CellPlasmDatabase.g = cellPlasm;
			CellWallDatabase.g = cellWall;
		}

		public void Build()
		{
			foreach (var _database in GetDatabases())
				_database.Build();
		}
	}
}