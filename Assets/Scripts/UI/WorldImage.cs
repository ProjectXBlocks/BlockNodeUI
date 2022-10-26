using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XBlocks.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class WorldImage : MonoBehaviour
    {
        public RectTransform parent, rect;

        void Update()
        {
            if (parent == null) return;
            float mx = parent.rect.width < parent.rect.height ? parent.rect.height : parent.rect.width;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mx);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mx);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            rect = GetComponent<RectTransform>();
        }
#endif
    }
}
