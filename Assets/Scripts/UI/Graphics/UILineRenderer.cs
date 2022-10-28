using UnityEngine;
using UnityEngine.UI;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic
    {
        public Vector2 start, end;
        public float stroke = 3f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Lines.Reset();
            Lines.vertex.color = color;
            Lines.stroke = stroke;
            Lines.Line(vh, start.x, start.y, end.x, end.y);
        }
    }
}
