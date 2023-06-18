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
    public LifeController life;
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
    public float jumpTeleport;
    public float jumpRecover;
    private float jumpNext;

    private MovementSettings current;

    // Start is called before the first frame update
    void Start()
    {
        gd = GetComponent<GroundDetector>();
        life = GetComponent<LifeController>();
        rb = GetComponent<Rigidbody2D>();
        current = ground;
        feetMaterial = new PhysicsMaterial2D();
        feet.sharedMaterial = feetMaterial;

        GameManager.scripts.Add(GameManager.SCRIPTS.PlayerMovement, this);
        GameManager.scripts.Add(GameManager.SCRIPTS.PlayerLifeController, life);
    }

    // Update is called once per frame
    void Update()
    {
        if (gd.grounded)
        {
            current = MovementSettings.Lerp(current, ground, ground.transitionSpeed * Time.deltaTime);
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                current = MovementSettings.Lerp(current, air_down, ground.transitionSpeed * Time.deltaTime);
            }
            else
            {
                current = MovementSettings.Lerp(current, air_up, ground.transitionSpeed * Time.deltaTime);
            }
        }

        if (gd.grounded && jumpNext < Time.time && Input.GetButtonDown("Jump"))
        {
            current.drag = air_up.drag;
            transform.position += Vector3.up * jumpTeleport;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpNext = Time.time + jumpRecover;
        }
        if (Input.GetKey("escape"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            dir = 1;
        }
        if (horizontal < 0)
        {
            dir = -1;
        }
        rb.AddForce(new Vector2(horizontal * current.speed * Time.fixedDeltaTime, 0));
        rb.drag = current.drag;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -current.speedMax, current.speedMax), rb.velocity.y);
        feetMaterial.friction = current.friction;
    }
}