using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsExtended
{
    [Serializable]
    public class SphereCastAverager
    {
        public Transform transform;
        public RaySettings raySettings = new RaySettings(Vector3.zero, Vector3.down, .1f);
        [Min(0f)] public float radius = .5f;
        public LayerMask collideLayers;
        public QueryTriggerInteraction queryTriggerInteraction;
        public bool ignoreInside = true;
        public bool ignoreSelf = true;
        public bool ignoreChildren = false;
        [Range(-1,1)]public float ignoreSlope = 0;
        public bool ignoreSlopeDiscard = false;
        private RaycastHit[] hits;
        private List<RaycastHit> hitsValid = new List<RaycastHit>();
        private bool hittedLast;
        private bool hitted;
        public bool Hitted
        {
            get => hitted;
        }
        public bool HittedLast
        {
            get => hittedLast;
        }
        private Vector3 position;
        public Vector3 Position
        {
            get => position;
        }
        private Vector3 normal;
        public Vector3 Normal
        {
            get => normal;
        }
        public void Query()
        {
            Vector3 origin = raySettings.origin + transform.position;
            Vector3 direction = transform.TransformDirection(raySettings.direction);
            hits = Physics.SphereCastAll(origin, radius, direction, raySettings.length, collideLayers, queryTriggerInteraction);
            hitsValid.Clear();
            position = Vector3.zero;
            normal = Vector3.zero;
            int count = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if(PhysicsUtils.FixCast(origin, direction, ref hits[i], radius))
                {
                    if (ignoreInside)
                    {
#if UNITY_EDITOR
                        //Debug.Log(hits[i].point + ", " + hits[i].normal);
                        Debug.DrawRay(hits[i].point, hits[i].normal, new Color(.1f, 0, 0, .1f));
#endif
                        break;
                    }
                }
                if (ignoreSelf)
                {
                    if (hits[i].transform == transform)
                    {
#if UNITY_EDITOR
                        //Debug.Log(hits[i].point + ", " + hits[i].normal);
                        Debug.DrawRay(hits[i].point, hits[i].normal, new Color(.1f, 0, 0, .1f));
#endif
                        break;
                    }
                }
                if(ignoreSlope < 1f && ignoreSlopeDiscard)
                {
                    if(Vector3.Dot(hits[i].normal, direction) > ignoreSlope)
                    {
#if UNITY_EDITOR
                        //Debug.Log(hits[i].point + ", " + hits[i].normal);
                        Debug.DrawRay(hits[i].point, hits[i].normal, new Color(.1f, 0, 0, .1f));
#endif
                        break;
                    }
                }
                if (ignoreChildren)
                {
                    if (hits[i].transform.IsChildOf(transform))
                    {
#if UNITY_EDITOR
                        //Debug.Log(hits[i].point + ", " + hits[i].normal);
                        Debug.DrawRay(hits[i].point, hits[i].normal, new Color(.1f, 0, 0, .1f));
#endif
                        break;
                    }
                }
#if UNITY_EDITOR
                Debug.DrawRay(position, normal, new Color(0, .1f, 0, .1f));
                //Debug.Log(hits[i].point + ", " + hits[i].normal);
#endif
                position += hits[i].point;
                normal += hits[i].normal;
                hitsValid.Add(hits[i]);
                count++;
            }
            hittedLast = hitted;
            hitted = count > 0;
            if (hitted)
            {
                position /= (float)count;
                normal /= (float)count;
                normal.Normalize();
                hitted = Vector3.Dot(normal, direction) <= ignoreSlope;
            }
            if (hitted)
            {
#if UNITY_EDITOR
                DebugExtended.DrawCapsule(origin, direction, radius, raySettings.length + radius + radius, new Color(0, 1, 0, .5f), centered: false);
                //Debug.Log(count);
                Debug.DrawRay(position, normal, Color.green);
#endif
            }
            else
            {
#if UNITY_EDITOR
                DebugExtended.DrawCapsule(origin, direction, radius, raySettings.length + radius + radius, new Color(1, 0, 0, .5f), centered: false);
#endif
            }

        }

        public bool ContainsTag(string tagCompare)
        {
            foreach (RaycastHit hit in hitsValid)
            {
                if (hit.collider.CompareTag(tagCompare))
                    return true;
            }
            return false;
        }
    }
}
