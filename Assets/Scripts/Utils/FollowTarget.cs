using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float radius;
    public float speed;
    public Vector3 offset;
    public bool move_x;
    public bool move_y;
    public bool move_z;

    void Start()
    {
        SetPos();
    }
    public void SetPos()
    {
        Vector3 newPos = target.position + offset;

        newPos = new Vector3(move_x ? newPos.x : transform.position.x, move_y ? newPos.y : transform.position.y, move_z ? newPos.z : transform.position.z);

        transform.position = newPos;
    }
    void Update()
    {
        Vector3 desiredpos = target.position + offset;

        desiredpos = new Vector3(move_x ? desiredpos.x : transform.position.x, move_y ? desiredpos.y : transform.position.y, move_z ? desiredpos.z : transform.position.z);

        Vector3 newPos = transform.position;

        newPos = Vector3.Lerp(newPos, desiredpos, speed * Time.deltaTime);

        transform.position = newPos;
    }
}
