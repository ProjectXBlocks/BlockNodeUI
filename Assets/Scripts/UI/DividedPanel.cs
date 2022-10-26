using UnityEngine;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [ExecuteAlways] [RequireComponent(typeof(RectTransform))] [AddComponentMenu("Layout/Divided Panel")]
    public class DividedPanel : MonoBehaviour
    {
        public RectTransform rect;
        public RectTransform reference;
        public PanelAlign align = PanelAlign.right;
        private float last = 0f;

        public enum PanelAlign
        {
            left,
            right,
            top,
            bottom
        }

        void Update()
        {
            if (Application.isPlaying || (rect != null && reference != null)) Set();
        }

        private void Set()
        {
            float v = (align == PanelAlign.left || align == PanelAlign.right) ? reference.rect.width : reference.rect.height;
            if(v == last) return;
            last = v;
            switch (align)
            {
                case PanelAlign.left:
                    rect.SetRight(v);
                    break;
                case PanelAlign.right:
                    rect.SetLeft(v);
                    break;
                case PanelAlign.top:
                    rect.SetBottom(v);
                    break;
                case PanelAlign.bottom:
                    rect.SetTop(v);
                    break;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            rect = GetComponent<RectTransform>();
        }
#endif
    }
}
