using Gem;
using UnityEngine;

namespace HX.UI
{
	public class GarageController : MonoBehaviour
	{
		private const float NEO_ANGULAR_VELOCITY = 5;
		private const float CAMERA_DEFAULT_SIZE = 1f;
		private const float CAMERA_POSSESS_SIZE = 3f;

		public static GarageController g { get; private set; }

		public bool isPossessed { get { return mNeoController != null; } }

		private Neo mNeo;
		private NeoController mNeoController;

		public GameObject uiRoot { get { return mUIRoot; } }
		public Camera uiCamera { get { return mUICamera; } }

		public GameObject worldRoot { get { return mWorldRoot; } }
		public Camera worldCamera { get { return mWorldCamera; } }
		
		[SerializeField] private GameObject mUIRoot;
		[SerializeField] private Camera mUICamera;
		[SerializeField] private GameObject mWorldRoot;
		[SerializeField] private Camera mWorldCamera;
		[SerializeField] private Transform mNeoRoot;

		void Start()
		{
			ReplaceNeo();

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
		}

		void Update()
		{
			var _dt = Time.deltaTime;

			if (!isPossessed)
			{
				mNeoRoot.transform.AddLEulerZ(_dt * NEO_ANGULAR_VELOCITY);
			}
			else
			{
				var _lerp = 4*_dt;

				mWorldCamera.orthographicSize = Mathf.Lerp(mWorldCamera.orthographicSize, CAMERA_POSSESS_SIZE, _lerp);

				var _neoTrans = mNeo.transform;
				var _camTrans = mWorldCamera.transform;

				var _newPos = Vector2.Lerp(_camTrans.position, _neoTrans.position, _lerp);
				_camTrans.SetPos(_newPos);

				var _newZ = Mathf.Lerp(_camTrans.eulerAngles.z, _neoTrans.eulerAngles.z, _lerp);
				_camTrans.SetEulerZ(_newZ);
			}
		}

		void ReplaceNeo()
		{
			if (mNeo)
				Destroy(mNeo.gameObject);

			mNeo = NeoUtil.InstantiateNeo();
			mNeo.transform.SetParent(mNeoRoot, false);
			mNeo.mechanics.Build(UserManager.neoStructure);

			var _boundingRect = mNeo.mechanics.boundingRect;
			mNeo.transform.localPosition = -_boundingRect.center;
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
			}
			else
			{
				Destroy(mNeoController);
				mNeoController = null;
				ReplaceNeo();

				mWorldCamera.transform.SetLPos(Vector2.zero);
				mWorldCamera.orthographicSize = CAMERA_DEFAULT_SIZE;
			}
		}

		public void OnClickPossessButton()
		{
			Possess(!isPossessed);
		}

		public Vector2 WorldToUI(Vector2 _globalPos)
		{
			var _viewport = worldCamera.WorldToViewportPoint(_globalPos);
			return uiCamera.ViewportToWorldPoint(_viewport);
		}

		public Vector2 UIToWorld(Vector2 _globalPos)
		{
			var _viewport = uiCamera.WorldToViewportPoint(_globalPos);
			return worldCamera.ViewportToWorldPoint(_viewport);
		}
	}
}
