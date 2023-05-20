using PhysicsExtended;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugExtended
{
    public static Color GizmosPreviousColor { get; private set; } = Color.white;
    public static void GizmosSaveColor()
    {
#if UNITY_EDITOR
        GizmosPreviousColor = Gizmos.color;
#endif
    }
    public static void GizmosRestoreColor()
    {
#if UNITY_EDITOR
        Gizmos.color = GizmosPreviousColor;
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"> center of the circle </param>
    /// <param name="normal"> normal vector (forward) </param>
    /// <param name="tangent"> tangent vector (up) </param>
    /// <param name="radius"> bitangent vector (right) </param>
    /// <param name="color"></param>
    /// <param name="completion"></param>
    /// <param name="steps"></param>
    public static void DrawCircle(Vector3 center, Vector3 normal, Vector3 tangent, float radius, Color color,
        float completion = 1, bool centered = false, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        normal.Normalize();
        tangent.Normalize();
        completion = Mathf.Clamp01(completion);
        if (centered)
        {
            tangent = Quaternion.AngleAxis(180f * (1 - completion), normal) * tangent;
        }

        if (steps < 8) steps = 8;
        float stepsIncrease = (360f * completion) / (float) steps;
        Vector3 lastPoint = Vector3.zero;
        Vector3 currentPoint = tangent * radius;
        if (drawRadius)
            Debug.DrawLine(center + lastPoint, center + currentPoint, color);
        for (int i = 0; i < steps; i++)
        {
            lastPoint = currentPoint;
            currentPoint = Quaternion.AngleAxis(stepsIncrease, normal) * currentPoint;
            Debug.DrawLine(center + lastPoint, center + currentPoint, color);
        }

        if (drawRadius)
            Debug.DrawLine(center, center + currentPoint, color);
#endif
    }

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color color, float completion = 1,
        bool centered = false, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        normal.Normalize();
        DrawCircle(center, normal, Utils.GetTangent(normal), radius, color, completion, centered, steps, drawRadius);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"> center of the sphere </param>
    /// <param name="normal"> normal vector (forward) </param>
    /// <param name="tangent"> tangent vector (up) </param>
    /// <param name="bitangent"> bitangent vector (right) </param>
    /// <param name="size"> size of the cube </param>
    /// <param name="color"></param>
    public static void DrawCube(Vector3 center, Vector3 normal, Vector3 tangent, Vector3 bitangent, Vector3 size,
        Color color)
    {
#if UNITY_EDITOR
        size *= .5f;
        normal *= size.z;
        tangent *= size.y;
        bitangent *= size.x;
        Debug.DrawLine(center + normal - tangent - bitangent, center - normal - tangent - bitangent, color);
        Debug.DrawLine(center + normal + tangent - bitangent, center - normal + tangent - bitangent, color);
        Debug.DrawLine(center + normal - tangent + bitangent, center - normal - tangent + bitangent, color);
        Debug.DrawLine(center + normal + tangent + bitangent, center - normal + tangent + bitangent, color);

        Debug.DrawLine(center + tangent - normal - bitangent, center - tangent - normal - bitangent, color);
        Debug.DrawLine(center + tangent + normal - bitangent, center - tangent + normal - bitangent, color);
        Debug.DrawLine(center + tangent - normal + bitangent, center - tangent - normal + bitangent, color);
        Debug.DrawLine(center + tangent + normal + bitangent, center - tangent + normal + bitangent, color);

        Debug.DrawLine(center + bitangent - normal - tangent, center - bitangent - normal - tangent, color);
        Debug.DrawLine(center + bitangent + normal - tangent, center - bitangent + normal - tangent, color);
        Debug.DrawLine(center + bitangent - normal + tangent, center - bitangent - normal + tangent, color);
        Debug.DrawLine(center + bitangent + normal + tangent, center - bitangent + normal + tangent, color);

        //Debug.DrawLine(center + normal, center - normal, color);
        //Debug.DrawLine(center + tangent, center - tangent, color);
        //Debug.DrawLine(center + bitangent, center - bitangent, color);
#endif
    }

    public static void DrawCube(Vector3 center, Vector3 direction, Vector3 size, Color color)
    {
#if UNITY_EDITOR
        direction.Normalize();
        Vector3 bitangent;
        Vector3 tangent = Utils.GetTangent(in direction, out bitangent);
        DrawCube(center, direction, tangent, bitangent, size, color);
#endif
    }

    public static void DrawCube(Transform transform, Vector3 size, Color color)
    {
#if UNITY_EDITOR
        DrawCube(transform.position, transform.forward, transform.up, transform.right, size, color);
#endif
    }

    public static void DrawCube(Transform transform, BoxColliderSettings collider, Color color)
    {
#if UNITY_EDITOR
        DrawCube(transform.position + collider.center, transform.forward, transform.up, transform.right, collider.size,
            color);
#endif
    }

    public static void DrawCube(BoxCollider collider, Color color)
    {
#if UNITY_EDITOR
        DrawCube(collider.transform, (BoxColliderSettings) collider, color);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"> center of the sphere </param>
    /// <param name="normal"> normal vector (forward) </param>
    /// <param name="tangent"> tangent vector (up) </param>
    /// <param name="bitangent"> bitangent vector (right) </param>
    /// <param name="radius"></param>
    /// <param name="color"></param>
    /// <param name="completion"></param>
    /// <param name="steps"></param>
    public static void DrawSphere(Vector3 center, Vector3 normal, Vector3 tangent, Vector3 bitangent, float radius,
        Color color, float completion = 1, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawCircle(center, normal, tangent, radius, color, completion, false, steps, drawRadius);
        DrawCircle(center, tangent, bitangent, radius, color, completion, false, steps, drawRadius);
        DrawCircle(center, bitangent, normal, radius, color, completion, false, steps, drawRadius);
#endif
    }

    public static void DrawSphere(Vector3 center, Vector3 direction, float radius, Color color, float completion = 1,
        ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        direction.Normalize();
        Vector3 bitangent;
        Vector3 tangent = Utils.GetTangent(in direction, out bitangent);
        DrawSphere(center, direction, tangent, bitangent, radius, color, completion, steps, drawRadius);
#endif
    }

    public static void DrawSphere(Transform transform, float radius, Color color, float completion = 1,
        ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawSphere(transform.position, transform.forward, transform.up, transform.right, radius, color, completion,
            steps, drawRadius);
#endif
    }

    public static void DrawSphere(Transform transform, SphereColliderSettings collider, Color color,
        float completion = 1, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawSphere(transform.position + collider.center, transform.forward, transform.up, transform.right,
            collider.radius, color, completion, steps, drawRadius);
#endif
    }

    public static void DrawSphere(SphereCollider collider, Color color, float completion = 1, ushort steps = 59,
        bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawSphere(collider.transform, (SphereColliderSettings) collider, color, completion, steps, drawRadius);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"> center of the sphere </param>
    /// <param name="normal"> normal vector (forward) </param>
    /// <param name="tangent"> tangent vector (up) </param>
    /// <param name="bitangent"> bitangent vector (right) </param>
    /// <param name="radius"></param>
    /// <param name="color"></param>
    /// <param name="steps"></param>
    public static void DrawHemiSphere(Vector3 center, Vector3 normal, Vector3 tangent, Vector3 bitangent, float radius,
        Color color, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawCircle(center, normal, tangent, radius, color, 1f, false, steps);
        DrawCircle(center, tangent, bitangent, radius, color, .5f, false, steps, drawRadius);
        DrawCircle(center, -bitangent, tangent, radius, color, .5f, false, steps, drawRadius);
#endif
    }

    public static void DrawHemiSphere(Vector3 center, Vector3 direction, float radius, Color color, ushort steps = 59,
        bool drawRadius = false)
    {
#if UNITY_EDITOR
        direction.Normalize();
        Vector3 bitangent;
        Vector3 tangent = Utils.GetTangent(in direction, out bitangent);
        DrawHemiSphere(center, direction, tangent, bitangent, radius, color, steps, drawRadius);
#endif
    }

    public static void DrawHemiSphere(Transform transform, float radius, Color color, ushort steps = 59,
        bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawHemiSphere(transform.position, transform.forward, transform.up, transform.right, radius, color, steps,
            drawRadius);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"> center of the sphere </param>
    /// <param name="normal"> normal vector (forward) </param>
    /// <param name="tangent"> tangent vector (up) </param>
    /// <param name="bitangent"> bitangent vector (right) </param>
    /// <param name="radius"></param>
    /// <param name="length"></param>
    /// <param name="color"></param>
    /// <param name="centered"></param>
    /// <param name="steps"></param>
    public static void DrawCapsule(Vector3 center, Vector3 normal, Vector3 tangent, Vector3 bitangent, float radius,
        float length, Color color, bool centered = true, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        float diameter = radius * 2;
        length -= diameter;
        if (length < 0)
            length = 0;
        normal.Normalize();
        if (centered)
        {
            center -= normal * length;
        }

        DrawHemiSphere(center, normal, tangent, bitangent, radius, color, steps, drawRadius);
        DrawHemiSphere(center + normal * length, normal, tangent, bitangent, -radius, color, steps, drawRadius);
        Debug.DrawLine(center + tangent * radius, center + normal * length + tangent * radius, color);
        Debug.DrawLine(center + bitangent * radius, center + normal * length + bitangent * radius, color);
        Debug.DrawLine(center - tangent * radius, center + normal * length - tangent * radius, color);
        Debug.DrawLine(center - bitangent * radius, center + normal * length - bitangent * radius, color);
#endif
    }

    public static void DrawCapsule(Vector3 center, Vector3 direction, float radius, float length, Color color,
        bool centered = true, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        direction.Normalize();
        Vector3 bitangent;
        Vector3 tangent = Utils.GetTangent(new Vector3(-direction.x, -direction.y, -direction.z), out bitangent);
        DrawCapsule(center, direction, tangent, bitangent, radius, length, color, centered, steps, drawRadius);
#endif
    }

    public static void DrawCapsule(Transform transform, float radius, float length, Color color, bool centered = true,
        ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawCapsule(transform.position, transform.forward, transform.up, transform.right, radius, length, color,
            centered, steps, drawRadius);
#endif
    }

    public static void DrawCapsule(Transform transform, CapsuleColliderSettings collider, Color color,
        ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        switch (collider.direction)
        {
            case CapsuleColliderSettings.Direction.X:
                DrawCapsule(transform.position + collider.center, transform.right, transform.up, -transform.forward,
                    collider.radius, collider.height, color, true, steps, drawRadius);
                break;
            case CapsuleColliderSettings.Direction.Y:
                DrawCapsule(transform.position + collider.center, transform.up, -transform.forward, transform.right,
                    collider.radius, collider.height, color, true, steps, drawRadius);
                break;
            default:
                DrawCapsule(transform.position + collider.center, transform.forward, transform.up, transform.right,
                    collider.radius, collider.height, color, true, steps, drawRadius);
                break;
        }
#endif
    }

    public static void DrawCapsule(CapsuleCollider collider, Color color, ushort steps = 59, bool drawRadius = false)
    {
#if UNITY_EDITOR
        DrawCapsule(collider.transform, (CapsuleColliderSettings) collider, color, steps, drawRadius);
#endif
    }
    //        public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height)
    //        {
    //#if UNITY_EDITOR
    //            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
    //            using (new Handles.DrawingScope(angleMatrix))
    //            {
    //                var pointOffset = (_height - (_radius * 2)) / 2;

    //                //draw sideways
    //                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
    //                Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
    //                Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
    //                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
    //                //draw frontways
    //                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
    //                Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
    //                Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
    //                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
    //                //draw center
    //                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
    //                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

    //            }
    //#endif
    //        }
}