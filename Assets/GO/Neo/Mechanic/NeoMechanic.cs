using Gem;
using UnityEngine;

namespace HX
{
	public class NeoMechanic : MonoBehaviour
	{
		public int mass = 1;
		public Vector2 com = Vector2.zero;

		[SerializeField]
		private NeoMechanicData mData;
		public NeoMechanicData data { get { return mData; } }

		public HexCoor coor { get; private set; }

		public Animator animator;

		#region construct/destruct

		private bool mIsDestroying = false;

		private void Awake()
		{
			EnableRigidbody();
			mDamageDetector = gameObject.AddComponent<DamageDetector>();
			mDamageDetector.onDetect += Damage;
			cohesionLeft = data.cohesion;
			durabilityLeft = data.durability;
		}

		private void OnDestroy()
		{
			mIsDestroying = true;
			Detach();
		}

		#endregion

		#region parent

		private NeoMechanics mParent;

		public NeoMechanics parent
		{
			get { return mParent; }
			private set
			{
				if (parent == value)
					return;

				mParent = value;

				if (mParent != null)
				{
					mDamageDetector.owner = mParent.neo.GetInstanceID();
				}
				else
				{
					mDamageDetector.owner = 0;
					coor = HexCoor.ZERO;
					transform.parent = null;
					cohesionLeft = 0;
				}
			}
		}

		public virtual bool SetParent(NeoMechanics _parent, HexCoor _coor)
		{
			if (parent)
			{
				Debug.LogWarning("already parent exists! ignore.");
				return false;
			}

			if (! _parent)
			{
				Debug.LogWarning("trying to set null parent! use detach instead. ignore.");
				return false;
			}

			DisableRigidbody();
			parent = _parent;
			coor = _coor;

			return true;
		}

		public virtual void Detach()
		{
			if (!mIsDestroying)
				EnableRigidbody();
			parent = null;
		}

		#endregion

		#region neighbor

		protected void AddCohesion(NeoMechanic _mechanic)
		{
			cohesionLeft += _mechanic.data.cohesion/6f;
		}

		protected void RemoveCohesion(NeoMechanic _mechanic)
		{
			cohesionLeft -= _mechanic.data.cohesion/6f;
			UpdateLife();
		}

		#endregion

		#region rigidbody

		private void EnableRigidbody()
		{
			if (GetComponent<Rigidbody2D>()) return;
			gameObject.AddComponent<Rigidbody2D>();
			GetComponent<Rigidbody2D>().mass = mass;
			GetComponent<Rigidbody2D>().centerOfMass = com;
			GetComponent<Rigidbody2D>().drag = 1;
			GetComponent<Rigidbody2D>().angularDrag = 1;
		}

		private void DisableRigidbody()
		{
			if (!GetComponent<Rigidbody2D>()) return;
			Destroy(gameObject.GetComponent<Rigidbody2D>());
		}

		#endregion

		#region life

		public float durabilityLeft { get; private set; }
		public float cohesionLeft { get; private set; }
		private DamageDetector mDamageDetector;

		public void Damage(Damage _attackData)
		{
			if (cohesionLeft > 0 && parent)
				cohesionLeft -= _attackData;
			else if (durabilityLeft > 0)
				durabilityLeft -= _attackData;
			UpdateLife();
		}

		private void UpdateLife()
		{
			if (cohesionLeft <= 0 && parent)
				parent.Remove(this);

			if (durabilityLeft <= 0)
			{
				if (parent) parent.Remove(this);
				Decay();
			}
		}

		public void Decay()
		{
			Destroy(gameObject, 1);
		}

		#endregion
	}
}