using Gem;
using UnityEngine;

namespace HX
{
	public class Cell : MonoBehaviour
	{
		#region plasm

		private CellPlasm mPlasm;

		public CellPlasm plasm
		{
			get { return mPlasm; }
			set
			{
				if (plasm == value)
					return;

				if (plasm && value)
				{
					Debug.LogError("You cannot change plasm. Ignore.");
					return;
				}

				var _plasmOld = mPlasm;
				mPlasm = value;

				if (plasm)
				{
					plasm.transform.SetParentWithoutScale(transform);
					plasm.transform.localPosition = Vector3.zero;
					plasm.postDecay += OnPlasmDecay;
					if (wall) plasm.SetDamagable(false);
				}
				else
				{
					_plasmOld.postDecay -= OnPlasmDecay;
				}
			}
		}

		private void OnPlasmDecay(CellPlasm _plasm)
		{
			plasm = null;
			Destroy(this, CellConst.CELL_DECAY_DELAY);
		}

		#endregion

		#region wall

		private CellWall mWall;

		public CellWall wall
		{
			get { return mWall; }
			set
			{
				if (wall == value)
					return;

				if (wall && value)
				{
					Debug.LogError("You cannot change wall. Ignore.");
					return;
				}

				var _wallOld = mWall;
				mWall = value;

				if (wall)
				{
					wall.transform.SetParentIdentity(transform);
					wall.transform.localPosition = Vector3.zero;
					wall.postDecay += OnWallDecay;
					if (plasm) plasm.SetDamagable(false);
				}
				else
				{
					_wallOld.postDecay -= OnWallDecay;
					if (plasm) plasm.SetDamagable(true);
				}
			}
		}

		private void OnWallDecay(CellWall _wall)
		{
			wall = null;
		}

		#endregion

		#region grid

		public CellGrid grid { get; private set; }
		public HexNode<Cell> node { get; private set; }

		public bool SetGrid(CellGrid _grid, HexNode<Cell> _node)
		{
			if (grid)
			{
				Debug.LogWarning("Already grid exists! Ignore.");
				return false;
			}

			if (!_grid)
			{
				Debug.LogWarning("Trying to set null grid! Ignore.");
				return false;
			}

			grid = _grid;
			node = _node;

			return true;
		}

		public void DetachGrid()
		{
			grid = null;
			node = null;
		}

		#endregion
	}
}