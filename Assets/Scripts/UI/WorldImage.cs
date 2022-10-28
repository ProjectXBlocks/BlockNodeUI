using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XBlocks.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class WorldImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public RectTransform parent, rect;
        
        public static bool mouseDown = false;

        [Header("Camera")]
        public Transform worldCamera;
        [SerializeField] private float rotationSpeed = 500;
        [SerializeField] private Vector2 pitchAngleLimit = new Vector2(-30, 60f);

        private Vector2 rotation;
        private bool hasMouse = false;

        private void Start()
        {
            rotation = worldCamera.transform.eulerAngles;
        }

        private void Update()
        {
            if (parent == null) return;
            float mx = parent.rect.width < parent.rect.height ? parent.rect.height : parent.rect.width;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mx);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mx);

            if (Input.GetMouseButton(1))
            {
                if(mouseDown) RotateCamera();
                else if (hasMouse)
                {
                    mouseDown = true;
                    RotateCamera();
                }
            }
            else
            {
                mouseDown = false;
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            hasMouse = true;
        }

        public void OnPointerExit(PointerEventData data)
        {
            hasMouse = false;
        }

        private void RotateCamera()
        {
            rotation.y += Input.GetAxis("Mouse X") * 1 / 60 * rotationSpeed;
            rotation.y = rotation.y % 360;
            rotation.x += -Input.GetAxis("Mouse Y") * 1 / 60 * rotationSpeed;
            rotation.x = Mathf.Clamp(rotation.x, pitchAngleLimit.x, pitchAngleLimit.y);
            worldCamera.transform.eulerAngles = rotation;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            rect = GetComponent<RectTransform>();
        }
#endif
    }
}
