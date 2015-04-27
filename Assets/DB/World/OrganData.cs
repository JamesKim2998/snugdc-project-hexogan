using UnityEngine;

namespace HX
{
	public enum OrganKey
	{
		HUMAN_HEAD = 1,
		HUMAN_BREAST = 2,
		HUMAN_FOOT = 3,
	}

	public class OrganData : ScriptableObject
	{
		public OrganKey key;
		
		public Vector2 vertexPosition;

		public static implicit operator OrganKey(OrganData _this)
		{
			return _this.key;
		}
	}
}