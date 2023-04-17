using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    [Serializable]
    public struct Destination
    {
        public Transform target;
        public float duration;
    }

    public Destination[] destinations;
    [Min(-1)]
    public int destinationTarget;
    int destinationTargetPrev;
    [Range(0f, 1f)]
    public float destinationTraversal;

    void Start()
    {
        if(destinations.Length > 0)
        {
            destinationTarget = 0;
            destinationTargetPrev = destinations.Length - 1;
            transform.position = destinations[destinationTarget].target.position;
        }
        else
        {
            destinationTarget = -1;
            destinationTargetPrev = -1;
        }
    }

    void FixedUpdate()
    {
        if(destinationTarget > -1)
        {
            if (destinationTarget >= destinations.Length)
            {
                destinationTarget = 0;
                destinationTargetPrev = destinations.Length - 1;
            }
            transform.position = Vector3.Lerp(destinations[destinationTargetPrev].target.position, destinations[destinationTarget].target.position, destinationTraversal);
            destinationTraversal += Time.fixedDeltaTime / destinations[destinationTarget].duration;
            if(destinationTraversal >= 1)
            {
                destinationTraversal = 0;
                destinationTargetPrev = destinationTarget;
                destinationTarget++;
            }
        }
    }
}
