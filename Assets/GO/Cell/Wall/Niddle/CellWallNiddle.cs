using UnityEngine;

namespace HX
{
	[RequireComponent(typeof(CellWall))]
	public class CellWallNiddle : MonoBehaviour
	{
		private CellWall mWall;
		public DamageField damageField;

		void Awake()
		{
			mWall = GetComponent<CellWall>();
			mWall.onDataSetuped += OnDataSetuped;
		}

		void OnDestroy()
		{
			mWall.onDataSetuped -= OnDataSetuped;
		}

		private void OnDataSetuped(CellWall _wall, CellWallData _data)
		{
			var _niddleData = _data.GetProperty<CellWallNiddleData>();
			damageField.damage = _niddleData.damage;
		}
	}
}