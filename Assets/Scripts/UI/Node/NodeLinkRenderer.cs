using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class NodeLinkRenderer : Graphic
    {
        private const float NODE_STROKE = 3f;
        public Node node;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Lines.Reset();
            Lines.vertex.color = Color.red; //todo
            Lines.stroke = NODE_STROKE;

            if(node.type == Node.NodeType.sender && node.receivers.Count > 0)
            {
                foreach(Node rec in node.receivers)
                {
                    Lines.NodeLink(vh, node.Pos() - transform.position, rec.Pos() - transform.position);
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (node == null) node = GetComponent<Node>();
            raycastTarget = false;
        }
#endif
    }
}
