using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XBlocks.Utils
{
    //implementation by @Anuken (https://github.com/Anuken/Arc/blob/master/arc-core/src/arc/graphics/g2d/Lines.java)
    public static class Lines
    {
        public static UIVertex vertex = UIVertex.simpleVert;
        public static float stroke = 1f;

        private static Vector2 q1, q2, q3, q4;
        private static List<Vector2> vbuilder = new List<Vector2>();
        private static bool building = false;

        public static void Reset()
        {
            vertex = UIVertex.simpleVert;
            stroke = 1f;
        }

        public static void Line(VertexHelper vh, float x, float y, float x2, float y2, bool cap = false)
        {
            float hstroke = stroke / 2f;
            float len = Mathf.Sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
            float diffx = (x2 - x) / len * hstroke, diffy = (y2 - y) / len * hstroke;

            if (cap)
            {
                Fill.Quad(
                vh,

                x - diffx - diffy,
                y - diffy + diffx,

                x - diffx + diffy,
                y - diffy - diffx,

                x2 + diffx + diffy,
                y2 + diffy - diffx,

                x2 + diffx - diffy,
                y2 + diffy + diffx

                );
            }
            else
            {
                Fill.Quad(
                vh,

                x - diffy,
                y + diffx,

                x + diffy,
                y - diffx,

                x2 + diffy,
                y2 - diffx,

                x2 - diffy,
                y2 + diffx

                );
            }
        }

        public static void PolyLine(VertexHelper vh, List<Vector2> points, bool wrap = false)
        {
            int length = points.Count;
            Vector2 A, B = Vector2.zero, C = Vector2.zero, D, E, D0 = Vector2.zero, E0 = Vector2.zero;
            if (length < 2) return;

            float halfWidth = 0.5f * stroke;
            bool open = !wrap;

            for (int i = 1; i < length - 1; i++)
            {
                A = points[i-1];
                B = points[i];
                C = points[i+1];

                PreparePointyJoin(A, B, C, out D, out E, halfWidth);
                float x3 = D.x, y3 = D.y;
                float x4 = E.x, y4 = E.y;

                q3 = D;
                q4 = E;

                if (i == 1)
                {
                    if (open)
                    {
                        PrepareFlatEndpoint(points[1], points[0], out D, out E, halfWidth);
                        q1 = E;
                        q2 = D;
                    }
                    else
                    {
                        PreparePointyJoin(points[length - 1], A, B, out D0, out E0, halfWidth);

                        q1 = E0;
                        q2 = D0;
                    }
                }

                PushQuad(vh);
                q1 = new Vector2(x4, y4);
                q2 = new Vector2(x3, y3);
            }

            if (open)
            {
                //draw last link on path
                PrepareFlatEndpoint(B, C, out D, out E, halfWidth);
                q3 = E;
                q4 = D;
                PushQuad(vh);
            }
            else
            {
                //draw last link on path
                A = points[0];
                PreparePointyJoin(B, C, A, out D, out E, halfWidth);
                q3 = D;
                q4 = E;
                PushQuad(vh);

                //draw connection back to first vertex
                q1 = D;
                q2 = E;
                q3 = E0;
                q4 = D0;
                PushQuad(vh);
            }
        }

        public static void BeginLine()
        {
            if (!building)
            {
                building = true;
                vbuilder.Clear();
                
            }
            else throw new System.InvalidOperationException("Already building a line");
        }

        public static void LinePoint(Vector2 v)
        {
            if (building)
            {
                vbuilder.Add(v);
            }
            else throw new System.InvalidOperationException("Not building a line");
        }

        public static void EndLine(VertexHelper vh, bool wrap = false)
        {
            if (building)
            {
                PolyLine(vh, vbuilder, wrap);
                building = false;
            }
            else throw new System.InvalidOperationException("Not building a line");
        }

        public static void NodeLink(VertexHelper vh, Vector2 start, Vector2 end)
        {
            BeginLine();
            Vector2 offset = 30f * Vector2.right;
            LinePoint(start);
            LinePoint(start + offset);
            LinePoint(end - offset);
            LinePoint(end);
            EndLine(vh);
        }

        private static void PushQuad(VertexHelper vh)
        {
            Fill.Quad(vh, q1, q2, q3, q4);
        }

        private static void PrepareFlatEndpoint(Vector2 pathPoint, Vector2 endPoint, out Vector2 D, out Vector2 E, float halfLineWidth)
        {
            Vector2 v = (endPoint - pathPoint).normalized * halfLineWidth;
            D = new Vector2(v.y, -v.x) + endPoint;
            E = new Vector2(-v.y, v.x) + endPoint;
        }

        private static float PreparePointyJoin(Vector2 A, Vector2 B, Vector2 C, out Vector2 D, out Vector2 E, float halfLineWidth)
        {
            Vector2 AB = B - A;
            Vector2 BC = C - B;
            float angle = AngleRad(AB, BC);
            if (Mathf.Approximately(angle, 0) || Mathf.Approximately(angle, Mathf.PI) || Mathf.Approximately(angle, -Mathf.PI))
            {
                PrepareStraightJoin(AB, B, out D, out E, halfLineWidth);
                return angle;
            }
            float len = (float)(halfLineWidth / Mathf.Sin(angle));
            bool bendsLeft = angle < 0;
            AB = AB.SetLength(len);
            BC = BC.SetLength(len);
            Vector2 insidePoint = B - AB + BC;
            Vector2 outsidePoint = B + AB - BC;
            if (bendsLeft)
            {
                D = insidePoint;
                E = outsidePoint;
            }
            else
            {
                D = outsidePoint;
                E = insidePoint;
            }
            return angle;
        }

        private static void PrepareStraightJoin(Vector2 AB, Vector2 B, out Vector2 D, out Vector2 E, float halfLineWidth)
        {
            AB = AB.SetLength(halfLineWidth);
            D = new Vector2(-AB.y, AB.x) + B;
            E = new Vector2(AB.y, -AB.x) + B;
        }

        private static float AngleRad(Vector2 v, Vector2 reference)
        {
            //return Mathf.Atan2(v.x * reference.x + v.y * reference.y, reference.x * v.y - reference.y * v.x);
            return Vector2.SignedAngle(reference, v) * Mathf.Deg2Rad;
        }
    }
}
