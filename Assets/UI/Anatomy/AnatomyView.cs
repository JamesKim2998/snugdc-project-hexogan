using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace HX.UI
{
	using Graph = Dictionary<OrganKey, AnatomyView.Cluster>;

	public class AnatomyView : MonoBehaviour
	{
		internal class Cluster
		{
			public readonly AnatomyVertexView vertex;
			public readonly List<AnatomyConnectionView> conns = new List<AnatomyConnectionView>();

			public Cluster(AnatomyVertexView v)
			{
				vertex = v;
			}
		}

		public AnatomyData data { get; private set; }

		private readonly Graph mGraph = new Graph();

		public AnatomyVertexView selected { get; private set; }

		class Highlights
		{
			private readonly List<AnatomyVertexView> mVertexs = new List<AnatomyVertexView>();
			private readonly List<AnatomyConnectionView> mConns = new List<AnatomyConnectionView>();
			
			public void Add(AnatomyVertexView v)
			{
				v.Highlight(true);
				mVertexs.Add(v);
			}

			public void Add(AnatomyConnectionView _conn)
			{
				_conn.Highlight(true);
				mConns.Add(_conn);
			}

			public void Clear()
			{
				foreach (var v in mVertexs)
					v.Highlight(false);
				mVertexs.Clear();

				foreach (var _conn in mConns)
					_conn.Highlight(false);
				mConns.Clear();
			}
		}

		private readonly Highlights mHighlights = new Highlights();

		public Action<AnatomyVertexView> onSelectVertex;

		public void Setup(AnatomyData _data)
		{
			D.Assert(data == null);
			D.Assert(_data != null);

			data = _data;

			foreach (var kv in data.vertexs)
			{
				var v = UIDB.g.anatomyVertexPrf.Instantiate();
				v.transform.SetParent(transform, false);
				v.Setup(kv.Value);
				v.onClick += Select;

				mGraph.Add(kv.Key, new Cluster(v));
			}

			foreach (var _conn in data.conns)
			{
				var _ca = mGraph.GetOrDefault(_conn.a);
				var _cb = mGraph.GetOrDefault(_conn.b);
				if (_ca == null || _cb == null)
				{
					L.E("node not exists.");
					continue;
				}

				var _connView = UIDB.g.anatomyConnectionPrf.Instantiate();
				_connView.transform.SetParent(transform, false);

				var _pa = (Vector2)_ca.vertex.transform.localPosition;
				var _pb = (Vector2)_cb.vertex.transform.localPosition;

				_connView.Pivot(_pa, _pb);

				_ca.conns.Add(_connView);
				_cb.conns.Add(_connView);
			}
		}

		void Select(AnatomyVertexView v)
		{
			if (selected == v)
				return;
			
			if (selected)
				selected.Select(false);

			selected = v; 

			selected.Select(true);

			mHighlights.Clear();

			var _cluster = mGraph[v.data];
			foreach (var _conn in _cluster.conns)
				mHighlights.Add(_conn);

			var _conns = data.GetConnectionsOf(selected.data);

			foreach (var _conn in _conns)
			{
				Cluster _connCluster;
				if (mGraph.TryGet(_conn, out _connCluster))
					mHighlights.Add(_connCluster.vertex);
			}

			onSelectVertex.CheckAndCall(v);
		}
	}
}