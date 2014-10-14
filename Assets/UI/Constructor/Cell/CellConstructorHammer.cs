using UnityEngine;
using System.Collections;

namespace ui
{
	[RequireComponent(typeof(MouseActor))]
	public class CellConstructorHammer : MonoBehaviour
	{

		public void Start()
		{
			var _hammer = GetComponent<MouseActor>();
			_hammer.act = DoSmash;
			_hammer.mask = LayerBits.CELL;
		}

		private static bool DoSmash(GameObject _target)
		{
			var _wall = _target.GetComponent<CellWall>();
			if (_wall)
			{
				Destroy(_wall.gameObject);
				return true;
			}
			
			var _plasm = _target.GetComponent<CellPlasm>();
			if (_plasm)
			{
				Destroy(_plasm.gameObject);
				return true;
			}

			return false;
		}
	}
}