using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Draggable Divider")]
    public class DraggableDivider : Selectable, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform rect, parent;
        public RectTransform first, second;
        public DividerMode mode = DividerMode.horizontal;

        public float pad = 20f;
        //private float last = -1f;
        public bool dragged = false;

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
            float v = mode == DividerMode.horizontal ? rect.offsetMin.x + parent.rect.width * rect.anchorMin.x + rect.rect.width / 2f : - rect.offsetMin.y + parent.rect.height * rect.anchorMin.y - rect.rect.height / 2f;
            //if (v == last && last != -1) return;
            //last = v;
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
            dragged = true;
        }

        public void OnDrag(PointerEventData data)
        {
            SetDraggedPosition(data);
        }

        public void OnEndDrag(PointerEventData data)
        {
            dragged = false;
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            Vector3 globalMousePos;
            float bmin, bmax, p;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, data.position, data.pressEventCamera, out globalMousePos))
            {
                if (mode == DividerMode.horizontal)
                {
                    p = globalMousePos.x;
                    bmin = parent.position.x - parent.rect.width / 2f + pad;
                    bmax = parent.position.x + parent.rect.width / 2f - pad;
                    rect.position = new Vector3(Mathf.Clamp(p, bmin, bmax), rect.position.y, rect.position.z);
                }
                else
                {
                    p = globalMousePos.y;
                    bmin = parent.position.y - parent.rect.height / 2f + pad;
                    bmax = parent.position.y + parent.rect.height / 2f - pad;
                    rect.position = new Vector3(rect.position.x, Mathf.Clamp(p, bmin, bmax), rect.position.z);
                }
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            rect = GetComponent<RectTransform>();
        }
#endif
    }
}
