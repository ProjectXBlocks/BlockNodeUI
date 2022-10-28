using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XBlocks.Utils
{
    public static class Extensions
    {
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static float XMin(this RectTransform rt)
        {
            return rt.position.x - rt.rect.width / 2f;
        }

        public static float XMax(this RectTransform rt)
        {
            return rt.position.x + rt.rect.width / 2f;
        }

        public static float YMin(this RectTransform rt)
        {
            return rt.position.y - rt.rect.height / 2f;
        }

        public static float YMax(this RectTransform rt)
        {
            return rt.position.y + rt.rect.height / 2f;
        }

        public static Vector2 SetLength(this Vector2 v, float length)
        {
            v = v.normalized * Mathf.Abs(length);
            return v;
        }
    }
}
