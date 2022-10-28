using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static XBlocks.Utils.Lines;

namespace XBlocks.Utils
{
    public static class Fill
    {
        private static UIVertex[] quadVerts = new UIVertex[4];

        public static void Quad(VertexHelper vh, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            //Debug.Log(string.Format("{0},{1} {2},{3} {4},{5} {6},{7}", x1, y1, x2, y2, x3, y3, x4, y4));
            Quad(vh, new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4));
        }

        public static void Quad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            vertex.position = a;
            quadVerts[0] = vertex;
            vertex.position = b;
            quadVerts[1] = vertex;
            vertex.position = c;
            quadVerts[2] = vertex;
            vertex.position = d;
            quadVerts[3] = vertex;
            vh.AddUIVertexQuad(quadVerts);
        }
    }
}
