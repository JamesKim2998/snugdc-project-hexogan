using UnityEngine;
using System.Collections;

public class CellInitializer : MonoBehaviour
{
	public Cell cellPrf;

	void Start () {
		Setup();
	}
	
	public void Setup()
	{
		CellDatabase.cellPrf = cellPrf;
	}
}
