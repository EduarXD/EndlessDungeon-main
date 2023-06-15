using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    public AttackPatterns attacks;
    public GridCamera gridCam;
    public Vector2 visibility = Vector2.one;
    public Rigidbody2D rb;
    public Animator anim;
    public PlayerAnimationController.TargetJoint2DManual[] targetJoints;
    public LifeController player;
    public LifeController life;
    private Vector2 lastVelocity = Vector2.zero;
    private float gravityScale;
    public Vector2 targetOffset = Vector2.up;
    public float currentSpeed { get; protected set; }
    public Vector2 target { get; protected set; }
    public float targetDelaySpeed;
    public Vector2 targetDelay { get; protected set; }
    public void Move(float deltaTime)
    {
        if (onCamera)
        {
            rb.AddForce(target * currentSpeed * deltaTime);
        }
    }
    public bool onCamera { get; protected set; } = false;
    public bool onCameraLast { get; protected set; } = false;
    public bool CalcCamera()
    {
        if (gridCam != null)
        {
            Bounds cameraBounds = new Bounds((Vector2)gridCam.transform.position, gridCam.size);
            Bounds visibilityBounds = new Bounds((Vector2)transform.position, visibility);
            return onCamera = cameraBounds.Intersects(visibilityBounds);
        }
        else { return onCamera = false; }
    }
    public float viewRange;
    public bool isInViewRange { get { return distance <= viewRange; } }
    public float attackRange;
    public bool isInAttackRange { get { return distance <= attackRange; } }
    public float distance { get; protected set; } = Mathf.Infinity;
    private float CalcDistance()
    {
        if (player != null)
        {
            return distance = Vector2.Distance(transform.position, player.transform.position);
        }
        else
        {
            return distance = Mathf.Infinity;
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        gridCam = GameManager.scripts[GameManager.SCRIPTS.GridCamera] as GridCamera;
        player = GameManager.scripts[GameManager.SCRIPTS.PlayerLifeController] as LifeController;
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        CalcCamera();
        onCameraLast = !onCamera;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        if (onCamera)
        {
            rb.gravityScale = gravityScale;
        }
        if(!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        onCameraLast = onCamera;
        CalcCamera();
        if(onCameraLast != onCamera)
        {
            if (onCamera)
            {
                rb.velocity = lastVelocity;
                rb.gravityScale = gravityScale;
            }
            else
            {
                lastVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            }
        }
        CalcDistance();
        anim.SetBool("isInViewRange", isInViewRange);
        anim.SetBool("isInAttackRange", isInAttackRange);
        anim.SetBool("invencible", life.invencible);
        targetDelay = Vector2.Lerp(targetDelay, target, targetDelaySpeed * Time.deltaTime);
        anim.SetFloat("target_x", targetDelay.x);
        anim.SetFloat("target_y", targetDelay.y);
        foreach (TargetJoint2DManual joint in targetJoints)
        {
            joint.Apply();
        }
    }
#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        DebugExtended.GizmosSaveColor();
        if (gridCam != null)
        {
            Gizmos.DrawWireCube((Vector2)gridCam.transform.position, gridCam.size);

            if (!Application.isPlaying)
            {
                CalcCamera();
            }
            if(onCamera)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireCube((Vector2)transform.position, visibility);
        }
        if (player != null)
        {
            CalcDistance();
            if (isInAttackRange)
            {
                Gizmos.color = Color.red;
            }
            else if (isInViewRange)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
        DebugExtended.GizmosRestoreColor();
    }
#endif
}
