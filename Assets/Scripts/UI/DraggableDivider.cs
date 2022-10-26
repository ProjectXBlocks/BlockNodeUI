using UnityEngine;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [ExecuteAlways] [RequireComponent(typeof(RectTransform))] [AddComponentMenu("Layout/Draggable Divider")]
    public class DraggableDivider : MonoBehaviour
    {
        public RectTransform rect, parent;
        public RectTransform first, second;
        public DividerMode mode = DividerMode.horizontal;
        private float last = -1f;

        public enum DividerMode
        {
            horizontal,
            vertical
        }

        private void Update()
        {
            if (rect != null && parent == null) parent = (RectTransform)rect.parent;
            if (Application.isPlaying || rect != null) Set();
        }

        private void Set()
        {
            float v = mode == DividerMode.horizontal ? rect.offsetMin.x + parent.rect.width/2f + rect.rect.width / 2f : - rect.offsetMin.y + parent.rect.height / 2f - rect.rect.height / 2f;
            if (v == last && last != -1) return;
            last = v;
            switch (mode)
            {
                case DividerMode.horizontal:
                    if (first != null) first.SetRight(parent.rect.width - v);
                    if(second != null) second.SetLeft(v);
                    break;
                case DividerMode.vertical:
                    if (first != null) first.SetBottom(parent.rect.height - v);
                    if (second != null) second.SetTop(v);
                    break;
            }
        }

        private float CenterX()
        {
            return rect.rect.x + rect.rect.width / 2;
        }

        private float CenterY()
        {
            return rect.rect.y + rect.rect.height / 2;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            rect = GetComponent<RectTransform>();
        }
#endif
    }
}
