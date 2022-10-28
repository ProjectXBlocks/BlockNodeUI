using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XBlocks.Utils;

namespace XBlocks.UI
{
    [ExecuteAlways]
    [AddComponentMenu("Layout/Boundary Resizer")]
    public class BoundaryResizer : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public RectTransform parent, viewport, center;
        public float pad = 200f;
        public float elasticity = 15f;
        private RectTransform rect;
        private Vector3 lastMousePos;

        void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        void Update()
        {
            if(!Application.isPlaying) UpdateBounds();
            else UpdateEdges();
        }

        public void OnBeginDrag(PointerEventData data)
        {
            UpdateBounds();
            lastMousePos = Input.mousePosition;
        }

        public void OnDrag(PointerEventData data)
        {
            center.position += Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
        }

        private void UpdateEdges()
        {
            //x
            float xo = viewport.XMin() - rect.XMin();
            float xi = viewport.XMax() - rect.XMax();
            if(xo < 0 && xi <= 0)
            {
                center.position += Vector3.right * xo * elasticity * Time.deltaTime;
            }
            else if(xi > 0 && xo >= 0)
            {
                center.position += Vector3.right * xi * elasticity * Time.deltaTime;
            }

            //y
            xo = viewport.YMin() - rect.YMin();
            xi = viewport.YMax() - rect.YMax();
            if (xo < 0 && xi <= 0)
            {
                center.position += Vector3.up * xo * elasticity * Time.deltaTime;
            }
            else if (xi > 0 && xo >= 0)
            {
                center.position += Vector3.up * xi * elasticity * Time.deltaTime;
            }
        }

        public void UpdateBounds()
        {
            float xmin = center.position.x - viewport.rect.width / 2f + pad, xmax = center.position.x + viewport.rect.width / 2f - pad, ymin = center.position.y - viewport.rect.height / 2f + pad, ymax = center.position.y + viewport.rect.height / 2f - pad;
            //float xmin = parent.position.x, xmax = parent.position.x, ymin = parent.position.y, ymax = parent.position.y;

            foreach (Transform t in parent)
            {
                //Vector3 p = t.position - parent.position;
                Vector3 p = t.position;
                if(p.x < xmin) xmin = p.x;
                if(p.x > xmax) xmax = p.x;
                if(p.y < ymin) ymin = p.y;
                if(p.y > ymax) ymax = p.y;
            }

            xmax += pad; ymax += pad;
            xmin -= pad; ymin -= pad;
            transform.position = new Vector3((xmin + xmax) / 2f, (ymin + ymax) / 2f, 0);
            RectTransform rect = ((RectTransform) transform);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xmax - xmin);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ymax - ymin);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            RectTransform rect = ((RectTransform)transform);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(rect.position, new Vector3(rect.rect.width, rect.rect.height, 0));
        }
#endif
    }
}
