using UnityEngine;

namespace HX.UI.Constructor
{
	[RequireComponent(typeof(MouseActor))]
	public class Hammer : MonoBehaviour
	{
		[SerializeField] private MouseActor mActor;

		void Start()
		{
			mActor.act = DoSmash;
			mActor.mask = LayerBits.NEO;
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