using UnityEngine;
using System.Collections;

namespace neo.ui.constructor
{
	public class Hammer : MonoBehaviour
	{
		public static LayerMask dropMask;

		void Update()
		{
			var _mousePos = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
			var _posOld = transform.localPosition;
			_mousePos.z = _posOld.z;
			transform.position = _mousePos;

			if (Input.GetMouseButtonUp(0))
				Smash();
		}

		void Smash()
		{
			var _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var _result = Physics2D.OverlapCircle(_mousePos, 0.1f, dropMask, -1, 1);
			if (!_result)
			{
				Destroy(gameObject);
				return;
			}

			var _body = _result.GetComponent<NeoBody>();
			if (_body)
			{
				if (_body.parent)
					_body.parent.Remove(_body, true);
				Destroy(_body.gameObject);
				return;
			}

			var _arm = _result.GetComponent<NeoArm>();
			if (_arm)
			{
				if (_arm.parent)
					_arm.parent.Remove(_arm);
				Destroy(_arm.gameObject);
				return;
			}

			Destroy(gameObject);
		}

	}
}
