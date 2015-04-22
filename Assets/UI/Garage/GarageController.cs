using Gem;
using UnityEngine;

namespace HX.UI.Garage
{
	public class GarageController : MonoBehaviour
	{
		private const float NEO_ROOT_REL_POS = -0.8f;
		private const float NEO_ANGULAR_VELOCITY = 5;

		public static GarageController g { get; private set; }
		
		[SerializeField] private CameraController mCameraController;
		[SerializeField] private GameObject mUIRoot;
		[SerializeField] private GameObject mWorldRoot;

		[SerializeField] private Transform mNeoRoot;

		[SerializeField] private NeoConstructor mConstructor;
		private float mConstructorRivetX;

		public bool isRiveted { get { return mNeoController == null; } }
		public bool isPossessed { get { return mNeoController != null; } }

		public new CameraController camera { get { return mCameraController; } }
		public GameObject uiRoot { get { return mUIRoot; } }
		public GameObject worldRoot { get { return mWorldRoot; } }

		private Neo mNeo;
		private NeoController mNeoController;

		void Start()
		{
			ReplaceNeo();

			mConstructorRivetX = mConstructor.transform.localPosition.x;

			GarageEvents.onAssemble += OnAssemble;

			if (!g)
				g = this;
			else
				D.Assert(false);
		}

		void OnDestory()
		{
			if (g == this)
				g = null;
			else 
				D.Assert(false);

			GarageEvents.onAssemble -= OnAssemble;
		}

		void Update()
		{
			if (isRiveted)
				UpdateRivetd();
			else
				UpdatePossessed();
		}

		void UpdateRivetd()
		{
			var _dt = Time.deltaTime;
			var _lerp = 2 * _dt;

			mNeoRoot.transform.SetLPosX(mCameraController.world.orthographicSize * NEO_ROOT_REL_POS);
			mNeoRoot.transform.AddLEulerZ(_dt * NEO_ANGULAR_VELOCITY);

			var _targetPos = CalNeoRivetPosition();
			mNeo.transform.SetLPos(Vector2.Lerp(mNeo.transform.localPosition, _targetPos, _lerp));

			var _constructorLerp = 4 * _dt;
			mConstructor.transform.LerpLPosX(mConstructorRivetX, _constructorLerp);
		}

		void UpdatePossessed()
		{
			var _dt = Time.deltaTime;
			var _lerp = 2 * _dt;

			mConstructor.transform.LerpLPosX(Const.RESOLUTION_X/2, _lerp);
		}

		Vector2 CalNeoRivetPosition()
		{
			var _boundingRect = mNeo.mechanics.boundingRect;
			return -_boundingRect.center;
		}

		void ReplaceNeo()
		{
			if (mNeo)
			{
				camera.neo = null;
				Destroy(mNeo.gameObject);
			}

			mNeo = NeoUtil.InstantiateNeo();
			mNeo.transform.SetParent(mNeoRoot, false);
			mNeo.mechanics.Build(AssemblyManager.blueprint);
			mNeo.transform.SetLPos(CalNeoRivetPosition());

			camera.neo = mNeo;
		}

		void Possess(bool _val)
		{
			if (isPossessed == _val)
			{
				L.W("try again.");
				return;
			}

			if (_val)
			{
				mNeo.transform.SetParent(mWorldRoot.transform);
				mNeoController = gameObject.AddComponent<NeoController>();
				mNeoController.neo = mNeo;
				camera.ZoomIn();
			}
			else
			{
				Destroy(mNeoController);
				mNeoController = null;
				ReplaceNeo();
				camera.ZoomOut();
			}
		}

		public void OnAssemble(AssembleCommand _command)
		{
			var _assembly = _command.assembly;
			switch (_assembly.mechanicType)
			{
				case NeoMechanicType.BODY:
					AssemblyManager.blueprint.TryAdd(_command.coor, (BodyAssembly)_assembly);
					break;
				case NeoMechanicType.ARM:
					AssemblyManager.blueprint.TryAdd(_command.body.coor, _command.side, (ArmAssembly)_assembly);
					break;
				default:
					D.Assert(false);
					break;
			}
		}

		public void OnClickPossessButton()
		{
			Possess(!isPossessed);
		}
	}
}
