using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CellWall))]
public class CellWallNiddle : MonoBehaviour
{
	private CellWall m_Wall;
	public DamageField damageField;

	void Awake ()
	{
		m_Wall = GetComponent<CellWall>();
		m_Wall.postDataSetuped += ListenDataSetuped;
	}

	void OnDestroy()
	{
		m_Wall.postDataSetuped -= ListenDataSetuped;
	}

	private void ListenDataSetuped(CellWall _wall, CellWallData _data)
	{
		var _niddleData = _data.GetComponent<CellWallNiddleData>();
		damageField.attackData = _niddleData.damage;
	}
}
