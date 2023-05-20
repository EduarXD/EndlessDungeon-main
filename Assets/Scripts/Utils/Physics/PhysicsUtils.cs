using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysicsExtended
{
    public static class PhysicsUtils
    {
        public static bool FixCast(Vector3 origin, Vector3 direction, ref RaycastHit hit, float minDistance = 0)
        {
            if (hit.distance <= 0 && hit.point == Vector3.zero)
            {
                hit.point = origin + direction * minDistance;
                hit.distance = minDistance;
                return true;
            }

            return false;
        }
    }

    [Serializable]
    public struct RaySettings
    {
        public RaySettings(float _length = Mathf.Infinity)
        {
            origin = Vector3.zero;
            direction = Vector3.forward;
            length = _length;
        }

        public RaySettings(Vector3 _origin, Vector3 _direction, float _length = Mathf.Infinity)
        {
            origin = _origin;
            direction = _direction;
            length = _length;
        }

        public RaySettings(Ray _ray, float _length = Mathf.Infinity)
        {
            origin = _ray.origin;
            direction = _ray.direction;
            length = _length;
            //ray = new Ray(origin, direction);
        }

        public Vector3 origin;
        public Vector3 direction;
        public float length;

        public void Get(in Ray _ray)
        {
            origin = _ray.origin;
            direction = _ray.direction;
        }

        public void Apply(ref Ray _ray)
        {
            _ray.origin = origin;
            _ray.direction = direction;
        }

        public static explicit operator RaySettings(in Ray _ray)
        {
            return new RaySettings(_ray);
        }

        public static explicit operator Ray(in RaySettings _settings)
        {
            return new Ray(_settings.origin, _settings.direction);
        }
    }

    [Serializable]
    public struct ColliderSettings
    {
        public bool enabled;
        public bool isTrigger;

        public ColliderSettings(bool trigger = false)
        {
            enabled = true;
            isTrigger = trigger;
        }

        public static explicit operator ColliderSettings(in Collider _collider)
        {
            ColliderSettings settings = new ColliderSettings();
            settings.Get(_collider);
            return settings;
        }

        //public static explicit operator Collider(in ColliderSettings _settings)
        //{
        //    Collider collider = new Collider();
        //    _settings.Apply(ref collider);
        //    return collider;
        //}
        public void Get(in Collider _collider)
        {
            enabled = _collider.enabled;
            isTrigger = _collider.isTrigger;
        }

        public void Apply(ref Collider _collider)
        {
            _collider.enabled = enabled;
            _collider.isTrigger = isTrigger;
        }

        public static ColliderSettings Lerp(in ColliderSettings A, in ColliderSettings B, float T)
        {
            ColliderSettings settings = new ColliderSettings();
            settings.enabled = Utils.Lerp(A.enabled, B.enabled, T);
            settings.isTrigger = Utils.Lerp(A.isTrigger, B.isTrigger, T);
            return settings;
        }
    }

    [Serializable]
    public struct CapsuleColliderSettings
    {
        public bool enabled;
        public bool isTrigger;
        public Vector3 center;
        [Min(0)] public float radius;
        [Min(0)] public float height;

        public enum Direction
        {
            X,
            Y,
            Z
        }

        public Direction direction;

        public CapsuleColliderSettings(bool trigger = false)
        {
            enabled = true;
            isTrigger = trigger;
            center = Vector3.zero;
            radius = .5f;
            height = 1f;
            direction = Direction.Y;
        }

        public static explicit operator CapsuleColliderSettings(in CapsuleCollider _collider)
        {
            CapsuleColliderSettings settings = new CapsuleColliderSettings();
            settings.Get(_collider);
            return settings;
        }

        //public static explicit operator CapsuleCollider(in CapsuleColliderSettings _settings)
        //{
        //    CapsuleCollider collider = new CapsuleCollider();
        //    _settings.Apply(ref collider);
        //    return collider;
        //}
        public void Get(in CapsuleCollider _collider)
        {
            enabled = _collider.enabled;
            isTrigger = _collider.isTrigger;
            center = _collider.center;
            radius = _collider.radius;
            height = _collider.height;
            direction = (Direction) _collider.direction;
        }

        public void Apply(ref CapsuleCollider _collider)
        {
            _collider.enabled = enabled;
            _collider.isTrigger = isTrigger;
            _collider.center = center;
            _collider.radius = radius;
            _collider.height = height;
            _collider.direction = (int) direction;
        }

        public static CapsuleColliderSettings Lerp(in CapsuleColliderSettings A, in CapsuleColliderSettings B, float T)
        {
            CapsuleColliderSettings settings = new CapsuleColliderSettings();
            settings.enabled = Utils.Lerp(A.enabled, B.enabled, T);
            settings.isTrigger = Utils.Lerp(A.isTrigger, B.isTrigger, T);
            settings.center = Vector3.Lerp(A.center, B.center, T);
            settings.radius = Mathf.Lerp(A.radius, B.radius, T);
            settings.height = Mathf.Lerp(A.height, B.height, T);
            settings.direction = T > 0.5f ? B.direction : A.direction;
            return settings;
        }
    }

    [Serializable]
    public struct SphereColliderSettings
    {
        public bool enabled;
        public bool isTrigger;
        public Vector3 center;
        [Min(0)] public float radius;

        public SphereColliderSettings(bool trigger = false)
        {
            enabled = true;
            isTrigger = trigger;
            center = Vector3.zero;
            radius = .5f;
        }

        public static explicit operator SphereColliderSettings(in SphereCollider _collider)
        {
            SphereColliderSettings settings = new SphereColliderSettings();
            settings.Get(_collider);
            return settings;
        }

        //public static explicit operator SphereCollider(in SphereColliderSettings _settings)
        //{
        //    SphereCollider collider = new SphereCollider();
        //    _settings.Apply(ref collider);
        //    return collider;
        //}
        public void Get(in SphereCollider _collider)
        {
            enabled = _collider.enabled;
            isTrigger = _collider.isTrigger;
            center = _collider.center;
            radius = _collider.radius;
        }

        public void Apply(ref SphereCollider _collider)
        {
            _collider.enabled = enabled;
            _collider.isTrigger = isTrigger;
            _collider.center = center;
            _collider.radius = radius;
        }

        public static SphereColliderSettings Lerp(in SphereColliderSettings A, in SphereColliderSettings B, float T)
        {
            SphereColliderSettings settings = new SphereColliderSettings();
            settings.enabled = Utils.Lerp(A.enabled, B.enabled, T);
            settings.isTrigger = Utils.Lerp(A.isTrigger, B.isTrigger, T);
            settings.center = Vector3.Lerp(A.center, B.center, T);
            settings.radius = Mathf.Lerp(A.radius, B.radius, T);
            return settings;
        }
    }

    [Serializable]
    public struct BoxColliderSettings
    {
        public bool enabled;
        public bool isTrigger;
        public Vector3 center;
        public Vector3 size;

        public BoxColliderSettings(bool trigger = false)
        {
            enabled = true;
            isTrigger = trigger;
            center = Vector3.zero;
            size = Vector3.one;
        }

        public static explicit operator BoxColliderSettings(in BoxCollider _collider)
        {
            BoxColliderSettings settings = new BoxColliderSettings();
            settings.Get(_collider);
            return settings;
        }

        //public static explicit operator BoxCollider(in BoxColliderSettings _settings)
        //{
        //    BoxCollider collider = new BoxCollider();
        //    _settings.Apply(ref collider);
        //    return collider;
        //}
        public void Get(in BoxCollider _collider)
        {
            enabled = _collider.enabled;
            isTrigger = _collider.isTrigger;
            center = _collider.center;
            size = _collider.size;
        }

        public void Apply(ref BoxCollider _collider)
        {
            _collider.enabled = enabled;
            _collider.isTrigger = isTrigger;
            _collider.center = center;
            _collider.size = size;
        }

        public static BoxColliderSettings Lerp(in BoxColliderSettings A, in BoxColliderSettings B, float T)
        {
            BoxColliderSettings settings = new BoxColliderSettings();
            settings.enabled = Utils.Lerp(A.enabled, B.enabled, T);
            settings.isTrigger = Utils.Lerp(A.isTrigger, B.isTrigger, T);
            settings.center = Vector3.Lerp(A.center, B.center, T);
            settings.size = Vector3.Lerp(A.size, B.size, T);
            return settings;
        }
    }
}
