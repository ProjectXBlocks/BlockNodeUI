using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class PolylineRenderer : Graphic
    {
        public List<Vector2> points = new List<Vector2>();
        public float stroke = 3f;
        public bool wrap = false;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (points.Count < 2) return;
            Lines.Reset();
            Lines.vertex.color = color;
            Lines.stroke = stroke;
            Lines.PolyLine(vh, points, wrap);
        }
    }
}
