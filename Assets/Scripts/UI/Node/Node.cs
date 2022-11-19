using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XBlocks.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent (typeof(GraphNode))]
    [AddComponentMenu("Node/Node")]
    public class Node : MonoBehaviour
    {
        private const float NODE_OFFSET = 10f;
        public NodeType type = NodeType.sender;

        [Header("UI")]
        public RectTransform rect;
        public Image knob;

        public List<Node> receivers = new List<Node>(); //only valid if node is a sender
        public GraphNode graphNode;

        public enum NodeType
        {
            sender,
            receiver
        }

        private void Update()
        {
            knob.transform.position = Pos();
        }

        public Vector3 Pos()
        {
            return rect.position + (type == NodeType.sender ? rect.rect.width / 2 - NODE_OFFSET : - rect.rect.width / 2 + NODE_OFFSET) * Vector3.right;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(rect == null) rect = GetComponent<RectTransform>();
            if(graphNode == null) graphNode = GetComponent<GraphNode>();
        }
#endif
    }
}
