using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Utils
{
    public static float PerlinNoise3d(Vector3 pos)
    {
        return PerlinNoise3d(pos.x, pos.y, pos.z);
    }

    public static float PerlinNoise3d(float x, float y, float z)
    {
        return (Mathf.PerlinNoise(x, y) + Mathf.PerlinNoise(y, z) + Mathf.PerlinNoise(x, z) + Mathf.PerlinNoise(y, x) +
                Mathf.PerlinNoise(z, y) + Mathf.PerlinNoise(z, x)) / 6f;
    }

    //public static float Map(float val, float valMin, float valMax, float outMin, float outMax)
    //{
    //    return (val - valMin) / (outMin - valMin) * (outMax - valMax) + valMax;
    //    //return outMin + (val - valMin) * (outMax - outMin) / (valMax - valMin);
    //}
    public static float Map(float input, Vector4 vec)
    {
        return Map(input, vec.x, vec.y, vec.z, vec.w);
    }

    public static float Map(float input, float inputMin, float inputMax, float min, float max)
    {
        if (inputMax - inputMin == 0)
            return input;
        return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
    }

    public static Component FindNearest(in Vector3 pos, in IEnumerable<Component> objects)
    {
        Component bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (var obj in objects)
        {
            Vector3 directionToTarget = obj.transform.position - pos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = obj;
            }
        }

        return bestTarget;
    }

    public static List<Component> FindInRange(in Vector3 pos, float radius, in IEnumerable<Component> objects)
    {
        List<Component> inRange = new List<Component>();
        radius *= radius;
        foreach (var obj in objects)
        {
            Vector3 directionToTarget = obj.transform.position - pos;
            if (directionToTarget.sqrMagnitude < radius)
            {
                inRange.Add(obj);
            }
        }

        return inRange;
    }

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static bool IsInLayer(int layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }
    //public static GameObject FindNearest(in Vector3 pos, in IEnumerable<GameObject> objects)
    //{
    //    GameObject bestTarget = null;
    //    float closestDistanceSqr = Mathf.Infinity;
    //    foreach (var obj in objects)
    //    {
    //        Vector3 directionToTarget = obj.transform.position - pos;
    //        float dSqrToTarget = directionToTarget.sqrMagnitude;
    //        if (dSqrToTarget < closestDistanceSqr)
    //        {
    //            closestDistanceSqr = dSqrToTarget;
    //            bestTarget = obj;
    //        }
    //    }
    //    return bestTarget;
    //}

    /// <summary>
    /// Get angle from a vector
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="t"></param>
    /// 90º
    /// 180º    0º
    ///     270º
    /// <returns>Angle</returns>
    public static float GetAngle(float x, float y, float t)
    {
        if (x == 0 && y == 0)
            return t;
        else if (x == 0 && y != 0)
            if (y > 0)
                return 90.0f;
            else
                return -90.0f;
        else if (x != 0 && y == 0)
            if (x > 0)
                return 0f;
            else
                return 180f;
        else
            return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    public static float CorrectAnglePosition(float currentAngle, float newAngle, float speedMultiplier)
    {
        float auxAngle = newAngle;
        if (auxAngle > 360)
            auxAngle = auxAngle % 360;
        if (auxAngle < -360)
            auxAngle = auxAngle % -360;
        float difference = auxAngle - currentAngle;
        /*
         * if difference > pi(180 in this case)
         * Dif between angles must change sign
         * Examples:
         * ********************************************
         * If we use 175 and -175
         * 175 - -175= 350
         * Enter first IF
         * 350 - 360 = 10;
         * Difference is clockwise
         * ********************************************
         * */

        if (difference >= 180)
        {
            difference -= 360;
        }
        else if (difference <= -180)
        {
            difference += 360;
        }

        if (difference > 160)
        {
            float tmp = speedMultiplier * 2;
            if (tmp > 1)
                tmp = 1;
            return (currentAngle + (difference) * tmp);

        }
        else if (difference <= 3 && difference >= -3)
        {
            return auxAngle;
        }
        else
        {
            return (currentAngle + (difference) * speedMultiplier);
        }
    }
    public static float ClampAngle (float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F)) {
            if (angle < -360F) {
                angle += 360F;
            }
            if (angle > 360F) {
                angle -= 360F;
            }        
        }
        return Mathf.Clamp (angle, min, max);
    }
    public static float ClampLocalAngle (float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F)) {
            if (angle < -360F) {
                angle += 360F;
            }
            if (angle > 360F) {
                angle -= 360F;
            }        
        }
        if (angle > 90)
            angle -= 360;
        return Mathf.Clamp (angle, min, max);
    }


    public static Vector3 GetSphereCoordinatesFromAngles(float verticalAngle, float horizontalAngle, Vector3 center,
        float radius)
    {
        Vector3 tmpVec = Vector3.zero;
        tmpVec.x = center.x +
                   radius * (Mathf.Cos(horizontalAngle * Mathf.Deg2Rad) * Mathf.Sin(verticalAngle * Mathf.Deg2Rad));
        tmpVec.z = center.z +
                   radius * (Mathf.Sin(horizontalAngle * Mathf.Deg2Rad) * Mathf.Sin(verticalAngle * Mathf.Deg2Rad));
        tmpVec.y = center.y + radius * (Mathf.Cos(verticalAngle * Mathf.Deg2Rad));
        return tmpVec;
    }


    public static void GetColorDir(Vector3 worldPos, Vector3 direction)
    {

        RaycastHit raycastHit;

        if (Physics.Raycast(worldPos, direction, out raycastHit))
        {
            //if (raycastHit.collider.tag == "Billboard")
            //{
            //    return;
            //}

            Renderer renderer = raycastHit.collider.GetComponent<MeshRenderer>();
            Texture2D texture2D = renderer.material.mainTexture as Texture2D;
            Vector2 pCoord = raycastHit.textureCoord;
            pCoord.x *= texture2D.width;
            pCoord.y *= texture2D.height;

            Vector2 tiling = renderer.material.mainTextureScale;
            Color color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x),
                Mathf.FloorToInt(pCoord.y * tiling.y));

            Debug.Log("Picked color : " + color);

        }
    }




    public static Vector4 GetTexelSize(in Camera cam)
    {
        return new Vector4(1f / (float) cam.pixelWidth, 1f / (float) cam.pixelHeight, (float) cam.pixelWidth,
            (float) cam.pixelHeight);
    }

    public static Vector4 GetTexelSize(in RenderTexture tex)
    {
        return new Vector4(1f / (float) tex.width, 1f / (float) tex.height, (float) tex.width, (float) tex.height);
    }

    public static Vector4 GetTexelSize(in Texture tex)
    {
        return new Vector4(1f / (float) tex.width, 1f / (float) tex.height, (float) tex.width, (float) tex.height);
    }

    public static Vector4 GetTexelSize(in Texture2D tex)
    {
        return new Vector4(1f / (float) tex.width, 1f / (float) tex.height, (float) tex.width, (float) tex.height);
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        return copy as T;
    }

    public static bool Lerp(bool a, bool b, float t)
    {
        return t > 0.5f ? b : a;
    }

    public static bool AxisLerp(bool top, bool bot, bool lef, bool rig, Vector2 t)
    {
        if (Mathf.Abs(t.y) < Mathf.Abs(t.x))
        {
            if (t.x >= 0)
            {
                return rig;
            }
            else
            {
                return lef;
            }
        }
        else
        {
            if (t.y >= 0)
            {
                return top;
            }
            else
            {
                return bot;
            }
        }
    }

    public static float AxisLerp(float top, float bot, float lef, float rig, Vector2 t)
    {
        float y = Mathf.Lerp(bot, top, t.y * .5f + .5f);
        float x = Mathf.Lerp(lef, rig, t.x * .5f + .5f);
        float comp = (Mathf.Abs(t.y) + Mathf.Abs(t.x)) * 0.5f;
        if (Mathf.Abs(t.y) < Mathf.Abs(t.x))
            return Mathf.Lerp(y, x, comp);
        else
            return Mathf.Lerp(x, y, comp);
    }

    public static Vector2 AxisLerp(Vector2 top, Vector2 bot, Vector2 lef, Vector2 rig, Vector2 t)
    {
        Vector2 y = Vector2.Lerp(bot, top, t.y * .5f + .5f);
        Vector2 x = Vector2.Lerp(lef, rig, t.x * .5f + .5f);
        if (Mathf.Abs(t.y) < Mathf.Abs(t.x))
            return Vector2.Lerp(y, x, Mathf.Abs(t.x));
        else
            return Vector2.Lerp(x, y, Mathf.Abs(t.y));
    }

    public static Vector3 AxisLerp(Vector3 top, Vector3 bot, Vector3 lef, Vector3 rig, Vector2 t)
    {
        Vector3 y = Vector3.Lerp(bot, top, t.y * .5f + .5f);
        Vector3 x = Vector3.Lerp(lef, rig, t.x * .5f + .5f);
        if (Mathf.Abs(t.y) < Mathf.Abs(t.x))
            return Vector3.Lerp(y, x, Mathf.Abs(t.x));
        else
            return Vector3.Lerp(x, y, Mathf.Abs(t.y));
    }

    public static bool Lerp2(bool tl, bool tr, bool bl, bool br, Vector2 t)
    {
        if (t.y > 0.5f)
        {
            return t.x > 0.5f ? br : bl;
        }
        else
        {
            return t.x > 0.5f ? tr : tl;
        }
    }

    public static Vector2 AxisToUv(Vector2 input)
    {
        Vector2 temp = Quaternion.Euler(new Vector3(0, 0, 135)) * input;
        temp.x = Mathf.Clamp01(temp.x + 0.5f);
        temp.y = Mathf.Clamp01(temp.y + 0.5f);
        return temp;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tl">0,0</param>
    /// <param name="tr">1,0</param>
    /// <param name="bl">0,1</param>
    /// <param name="br">1,1</param>
    /// <param name="T"></param>
    /// <returns></returns>
    public static float Lerp2(float tl, float tr, float bl, float br, Vector2 T)
    {
        float vTop = Mathf.Lerp(tl, tr, T.x);
        float vBot = Mathf.Lerp(bl, br, T.x);
        return Mathf.Lerp(vTop, vBot, T.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tl">0,0</param>
    /// <param name="tr">1,0</param>
    /// <param name="bl">0,1</param>
    /// <param name="br">1,1</param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector2 Lerp2(Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br, Vector2 t)
    {
        Vector2 vTop = Vector2.Lerp(tl, tr, t.x);
        Vector2 vBot = Vector2.Lerp(bl, br, t.x);
        return Vector2.Lerp(vTop, vBot, t.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tl">0,0</param>
    /// <param name="tr">1,0</param>
    /// <param name="bl">0,1</param>
    /// <param name="br">1,1</param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 Lerp2(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br, Vector2 t)
    {
        Vector3 vTop = Vector3.Lerp(tl, tr, t.x);
        Vector3 vBot = Vector3.Lerp(bl, br, t.x);
        return Vector3.Lerp(vTop, vBot, t.y);
    }

    public static Vector3 GetTangent(in Vector3 normal)
    {
        Vector3 tangent = Vector3.Cross(normal, Vector3.forward);
        if (tangent.magnitude == 0)
        {
            tangent = Vector3.Cross(normal, Vector3.up);
        }

        return tangent.normalized;
    }

    public static Vector3 GetTangent(in Vector3 normal, out Vector3 biTangent)
    {
        Vector3 tangent = GetTangent(normal);
        biTangent = Quaternion.AngleAxis(90f, normal) * tangent;
        return tangent;
    }
}
