﻿using System;
using System.Collections.Generic;
using Gem;
using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public class HarvestField : MonoBehaviour
	{
		public float harvestRadius = 0.2f;
		public float forceFactor = 1;

		[SerializeField, UsedImplicitly] private Collider2D mCollider;
		private readonly List<Harvestable> mTargets = new List<Harvestable>();

		public Action<HarvestField, Harvestable> onHarvest;

		public void TurnOn()
		{
			D.Assert(mTargets.Empty());
			mCollider.enabled = true;
			enabled = true;
		}

		public void TurnOff()
		{
			enabled = false;
			mCollider.enabled = false;

			foreach (var _target in mTargets)
				if (_target) _target.TryExitAggro();
			mTargets.Clear();	
		}

		void Update()
		{
			mTargets.RemoveAll(_target =>
			{
				if (_target == null) 
					return true;
				return UpdateTarget(_target);
			});
		}

		bool UpdateTarget(Harvestable _target)
		{
			// note: should not be scaled.
			var _delta = transform.position - _target.transform.position;
			var _distance = _delta.magnitude;
			var _normal = _delta/_distance;

			var _shouldBeHarvested = _distance < harvestRadius;
			if (_shouldBeHarvested)
			{
				_target.BeHarvested();
				// note: little bit dangerous, reentrance.
				onHarvest.CheckAndCall(this, _target);
				return true;
			}
			else
			{
				var _body = _target.GetComponent<Rigidbody2D>();
				var _distanceSQ = _distance*_distance;
				_body.AddForce(forceFactor * _body.mass * _normal / _distanceSQ);
				return false;
			}
		}

		void OnTriggerEnter2D(Collider2D _collider)
		{
			var _target = _collider.GetComponent<Harvestable>();
			if (!_target) return;

			var _aggroEntered = _target.TryEnterAggro(gameObject);
			if (!_aggroEntered) return;

			mTargets.Add(_target);
		}

		void OnTriggerExit2D(Collider2D _collider)
		{
			var _target = _collider.GetComponent<Harvestable>();
			if (!_target || !_target.isAggroed) return;

			var _aggroExited = _target.TryExitAggro();
			D.Assert(_aggroExited);

			// should be removed from target anyway.
			mTargets.Remove(_target);
		}
	}
}
