using System.Collections.Generic;
using Gem;
using UnityEngine;


namespace HX
{

	[RequireComponent(typeof (Neo))]
	[RequireComponent(typeof (Rigidbody2D))]
	public class NeoRigidbody : MonoBehaviour
	{
		public float speedLimit = 3;
		public float angularSpeedLimit = 720;

		[SerializeField] 
		private Rigidbody2D mRigidbody;
		public new Rigidbody2D rigidbody { get { return mRigidbody; } }

		private void Awake()
		{
			mRigidbody.mass = 0;
			mRigidbody.inertia = 0;
		}

		public void Update()
		{
			if (mIsMassDirty)
				BuildMass();
		}

		private class MassData
		{
			public int mass;
		}

		private bool mIsMassDirty = false;
		private readonly Dictionary<HexCoor, MassData> mMassDatas = new Dictionary<HexCoor, MassData>();
		private int mTotalMass;
		private HexCoor mMassWeightedDoubleCoor;

		public bool IsBelowSpeedLimit()
		{
			return mRigidbody.velocity.sqrMagnitude < speedLimit*speedLimit;
		}

		public bool IsBelowAngularSpeedLimit()
		{
			return Mathf.Abs(mRigidbody.angularVelocity) < angularSpeedLimit;
		}

		public void AddMass(int _mass, HexCoor _coor, HexIdx? _side = null)
		{
			mIsMassDirty = true;

			var _coorDouble = _coor*2;
			if (_side.HasValue) _coorDouble += _side.Value;

			mTotalMass += _mass;
			mMassWeightedDoubleCoor += _coorDouble*_mass;

			MassData _massData;
			if (mMassDatas.TryGetValue(_coorDouble, out _massData))
				_massData.mass += _mass;
			else
				mMassDatas.Add(_coorDouble, new MassData {mass = _mass});
		}

		public void BuildMass()
		{
			if (! mIsMassDirty) return;
			mIsMassDirty = false;

			mRigidbody.mass = mTotalMass;
			mRigidbody.centerOfMass = NeoHex.Position(mMassWeightedDoubleCoor) / 2 / mTotalMass;

			float _inertia = 0;
			foreach (var _massData in mMassDatas)
				_inertia += _massData.Value.mass*((Vector2) _massData.Key/2).sqrMagnitude;
			mRigidbody.inertia = _inertia;
		}

		public static implicit operator Rigidbody2D(NeoRigidbody _this)
		{
			return _this.mRigidbody;
		}
	}

}