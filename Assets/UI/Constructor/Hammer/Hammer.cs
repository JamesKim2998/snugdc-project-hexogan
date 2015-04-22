using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class Hammer : MonoBehaviour
	{
		[SerializeField] private MouseFollow mFollow;
		[SerializeField] private MouseActor mActor;

		void Start()
		{
			mFollow.camera = GarageController.g.camera.ui;

			mActor.act = DoSmash;
			mActor.mask = LayerBits.NEO;
			mActor.camera = GarageController.g.camera.world;
		}

		// returns whether smash hit valid object.
		private static bool DoSmash(GameObject _target)
		{
			var _body = _target.GetComponent<NeoBody>();
			if (_body)
			{
				TryDestroyBody(_body);
				return true;
			}

			var _arm = _target.GetComponent<NeoArm>();
			if (_arm)
			{
				TryDestroyArm(_arm);
				return true;
			}

			return false;
		}

		private static bool TryDestroyBody(NeoBody _body)
		{
			if (_body.type == NeoBodyType.CORE)
				return false;

			var _connectedBodies = 0;
			foreach (var _neighbor in _body.GetNeighbors())
			{
				if (_neighbor == null)
					continue;
				if (_neighbor.mechanicType == NeoMechanicType.ARM)
					return false;
				if (++_connectedBodies > 1)
					return false;
			}

			OnDisassemble(_body);

			if (_body.parent)
				_body.parent.Remove(_body, true);
			Destroy(_body.gameObject);

			return true;
		}


		private static bool TryDestroyArm(NeoArm _arm)
		{
			OnDisassemble(_arm);

			if (_arm.parent)
				_arm.parent.Remove(_arm);
			Destroy(_arm.gameObject);

			return true;
		}

		private static void OnDisassemble(NeoMechanic _mechanic)
		{
			var _cmd = new DisassembleCommand { mechanic = _mechanic };
			GarageEvents.onDisassemble.CheckAndCall(_cmd);
		}
	}
}