using UnityEngine;
using System.Collections;

namespace HX.UI
{
	[RequireComponent(typeof(MouseActor))]
	public class NeoConstructorHammer : MonoBehaviour
	{
		public void Start()
		{
			var _hammer = GetComponent<MouseActor>();
			_hammer.act = DoSmash;
			_hammer.mask = LayerBits.NEO;
		}

		private static bool DoSmash(GameObject _target)
		{
			var _body = _target.GetComponent<NeoBody>();
			if (_body)
			{
				if (_body.parent)
					_body.parent.Remove(_body, true);
				Destroy(_body.gameObject);
				return true;
			}

			var _arm = _target.GetComponent<NeoArm>();
			if (_arm)
			{
				if (_arm.parent)
					_arm.parent.Remove(_arm);
				Destroy(_arm.gameObject);
				return true;
			}

			return false;
		}
	}
}