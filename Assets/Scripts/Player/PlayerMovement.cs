using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    public GroundDetector gd;
    public Rigidbody2D rb;
    public Collider2D feet;
    private PhysicsMaterial2D feetMaterial;

    [Serializable]
    public struct MovementSettings
    {
        public float speed;
        public float speedMax;
        public float drag;
        public float friction;
        public float transitionSpeed;

        public static MovementSettings Lerp(MovementSettings a, MovementSettings b, float t)
        {
            return new MovementSettings
            {
                speed = Mathf.Lerp(a.speed, b.speed, t),
                speedMax = Mathf.Lerp(a.speedMax, b.speedMax, t),
                friction = Mathf.Lerp(a.friction, b.friction, t),
                drag = Mathf.Lerp(a.drag, b.drag, t)
            };
        }
    }

    public MovementSettings ground;
    public MovementSettings air_up;
    public MovementSettings air_down;

    public int dir = 1;

    public float jumpForce;
    public float jumpRecover;
    private float jumpNext;

    private MovementSettings current;

    // Start is called before the first frame update
    void Start()
    {
        gd = GetComponent<GroundDetector>();
        rb = GetComponent<Rigidbody2D>();
        current = ground;
        feetMaterial = new PhysicsMaterial2D();
        feet.sharedMaterial = feetMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (gd.grounded && jumpNext < Time.time && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jumpNext = Time.time + jumpRecover;
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if(horizontal > 0)
        {
            dir = 1;
        }
        if(horizontal < 0)
        {
            dir = -1;
        }
        if (gd.grounded)
        {
            current = MovementSettings.Lerp(current, ground, ground.transitionSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                current = MovementSettings.Lerp(current, air_up, ground.transitionSpeed * Time.fixedDeltaTime);
            }
            else
            {
                current = MovementSettings.Lerp(current, air_down, ground.transitionSpeed * Time.fixedDeltaTime);
            }
        }

        rb.drag = current.drag;
        rb.AddForce(new Vector2(horizontal * current.speed * Time.fixedDeltaTime, 0));
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -current.speedMax, current.speedMax), rb.velocity.y);
        feetMaterial.friction = current.friction;
    }
}