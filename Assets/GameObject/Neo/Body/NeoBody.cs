﻿using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX
{

	public class NeoBody : NeoMechanic
	{
		public NeoBodyType type;

		#region neighbor
		private readonly NeoMechanic[] m_Neighbors = new NeoMechanic[6];

		public NeoMechanic GetNeighbor(HexIdx _side)
		{
			return m_Neighbors[(int)_side];
		}

		public IEnumerator<NeoMechanic> GetNeighbors()
		{
			foreach (var i in HexHelper.GetIdxes())
				yield return GetNeighbor(i);
		}

		private void SetNeighbor(HexIdx _side, NeoMechanic _mechanic)
		{
			var _neighbor = GetNeighbor(_side);
			if (_neighbor && _mechanic)
			{
				Debug.LogError("There is already mechanic in side " + _side + ". Ignore.");
				return;
			}

			m_Neighbors[(int)_side] = _mechanic;

			if (_mechanic)
				AddCohesion(_mechanic);
			else
				RemoveCohesion(_neighbor);
		}

		public void RemoveNeighbor(HexIdx _side)
		{
			var _neighbor = GetNeighbor(_side);
			if (!_neighbor) return;

			SetNeighbor(_side, null);

			var _body = _neighbor.GetComponent<NeoBody>();
			if (_body) _body.RemoveNeighbor(_side.Opposite());

			var _arm = _neighbor.GetComponent<NeoArm>();
			if (_arm) parent.Remove(_arm);
		}

		#endregion

		#region mechanic neighbor
		public void AddBody(NeoBody _body, HexIdx _side)
		{
			if (GetNeighbor(_side) == _body)
				return;

			SetNeighbor(_side, _body);
			_body.AddBody(this, _side.Opposite());
		}

		public void AddArm(NeoArm _arm, HexIdx _side)
		{
			SetNeighbor(_side, _arm);
			_arm.Attach(this, _side);
		}

		public override void Detach()
		{
			foreach (var i in HexHelper.GetIdxes())
				RemoveNeighbor(i);
			base.Detach();
		}
		#endregion
	}

}