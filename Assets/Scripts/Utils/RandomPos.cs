using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    public Transform pos;
    public float speed;
    public float radius;
    public Vector2 changeRate;
    Vector3 target;
    float nextChange;

    void Update()
    {
        if(nextChange < Time.time)
        {
            nextChange = Time.time + Random.Range(changeRate.x, changeRate.y);
            target = transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
        }
        pos.position = Vector3.Lerp(pos.position, target, speed * Time.deltaTime);
    }
}
