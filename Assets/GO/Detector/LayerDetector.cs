using System;
using Gem;
using UnityEngine;

namespace HX
{
	public class LayerDetector : MonoBehaviour
	{
		public LayerMask layerMask;
		public Action<Collider2D> onDetect;

		void OnTriggerEnter2D(Collider2D _collision)
		{
			if (layerMask.Contains(_collision))
			{
				if (onDetect == null)
				{
					Debug.LogError(gameObject.name + " does not set terrain detector!");
					return;
				}

				onDetect(_collision);
			}
		}
	}
}