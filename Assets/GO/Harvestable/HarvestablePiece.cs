using JetBrains.Annotations;
using UnityEngine;

namespace HX
{
	public class HarvestablePiece : Harvestable
	{
		private const float DESTROY_DELAY = 0.5f;

		[SerializeField, UsedImplicitly]
		private Rigidbody2D mBody;

		[SerializeField, UsedImplicitly] 
		private Collider2D mCollider;

		[SerializeField, UsedImplicitly]
		private Animator mAnimator;

		public HarvestablePiece()
			: base(HarvestableType.ASSEMBLY_PIECE)
		{}

		protected override void DoEnterAggro()
		{
			base.DoEnterAggro();
			mAnimator.SetTrigger("enter_aggro");
		}

		protected override void DoExitAggro()
		{
			base.DoExitAggro();
			mAnimator.SetTrigger("exit_aggro");
		}

		protected override void DoBeHarvested()
		{
			mBody.drag = 3;
			mCollider.enabled = false;
			mAnimator.SetTrigger("harvest");
			Destroy(gameObject, DESTROY_DELAY);
		}
	}
}
