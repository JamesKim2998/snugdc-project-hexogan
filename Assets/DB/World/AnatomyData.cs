using System;
using System.Collections.Generic;
using FullInspector;

namespace HX 
{
	public enum AnatomyKey
	{
		HUMAN = 1,
	}

	public class AnatomyData : BaseScriptableObject
	{
		[Serializable]
		public struct Conn
		{
			public OrganData a, b;
		}

		public AnatomyKey key;
		public string tmxDir { get { return "TMX/" + key + "/"; } }
		public Dictionary<OrganKey, OrganData> vertexs;

		public List<Conn> conns;

		public List<OrganData> GetConnectionsOf(OrganKey _key)
		{
			var _ret = new List<OrganData>();

			foreach (var _conn in conns)
			{
				if (_conn.a == _key)
					_ret.Add(_conn.b);

				if (_conn.b == _key)
					_ret.Add(_conn.a);
			}

			return _ret;
		}
	}
}