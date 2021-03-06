using Gem;
using UnityEditor;
using UnityEngine;

namespace HX
{
	[CustomEditor(typeof(CellGrid))]
	public class CellGridEditor : Editor<CellGrid>
	{
		Vector2 Transform(Vector2 _val)
		{
			return target.transform.TransformPoint(_val);
		}

		void DrawCell(HexCoor _coor, Color _color, bool _drawLabel)
		{
			Handles.color = _color;

			var _vs = new Vector3[7];
			foreach (var v in HexHelper.GetVertexs())
				_vs[(int) v] = Transform(_coor + v);
			_vs[6] = Transform(_coor + (HexVertex)0);

			Handles.DrawPolyLine(_vs);

			if (_drawLabel)
			{
				var _pos = Transform(new Vector2(0, -0.2f)) + _coor;
				Handles.Label(_pos, _coor.ToString());	
			}
		}

		void DrawGrid()
		{
			var _sceneView = SceneView.currentDrawingSceneView;

			var _bl = _sceneView.camera.ViewportToWorldPoint(Vector2.zero);
			var _tr = _sceneView.camera.ViewportToWorldPoint(Vector2.one);

			_bl -= target.transform.position;
			_tr -= target.transform.position;

			var _width = _tr.x - _bl.x;

			foreach (var _kv in target.Overlaps(UnityHelper.MakeRect(_bl, _tr), true))
				DrawCell(_kv.Key, Color.gray, _width < 8);
		}

		void OnSceneGUI()
		{
			DrawGrid();
		}
	}
}