using Gem;
using UnityEngine;

namespace HX
{
	public static class NeoUtil
	{
		public static Neo InstantiateNeo()
		{
			var _neo = NeoDB.g.neoPrf.Instantiate();
			_neo.SetCore(NeoDB.g.neoBodyCorePrf.Instantiate());

			var _coreTrans = _neo.core.transform;
			_coreTrans.SetParent(_neo.transform, false);
			_neo.core.transform.localPosition = Vector3.zero;

			return _neo;
		}
	}
}