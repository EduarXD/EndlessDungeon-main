using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    public Vector2 speed_idle;
    public Vector2 speed_view;
    public Vector2 speed_view_change;
    public Vector2 speed_attack;
    public Vector2 speed_attack_change;
    float nextSpeedChange;
    public float flapForce;
    public float flapTime;
    float nextFlapTime;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (onCamera)
        {
            if (life.invencible)
            {
                currentSpeed = 0;
            }
            else
            {
                if (nextSpeedChange <= Time.time)
                {
                    if (isInAttackRange)
                    {
                        nextSpeedChange = Time.time + Random.Range(speed_attack_change.x, speed_attack_change.y);
                        currentSpeed = Random.Range(speed_attack.x, speed_attack.y);
                    }
                    else if (isInViewRange)
                    {
                        nextSpeedChange = Time.time + Random.Range(speed_view_change.x, speed_view_change.y);
                        currentSpeed = Random.Range(speed_view.x, speed_view.y);
                    }
                    else
                    {
                        currentSpeed = 0;
                    }
                }
            }
            if (isInAttackRange)
            {
                target = (((Vector2)player.transform.position + targetOffset) - (Vector2)transform.position).normalized;
                targetDelay = target;
            }
            else if (isInViewRange)
            {
                target = (((Vector2)player.transform.position + targetOffset) - (Vector2)transform.position).normalized;
                targetDelay = target;
            }
            else
            {
                currentSpeed = 0;
            }
            if (nextFlapTime <= Time.time)
            {
                nextFlapTime = Time.time + flapTime;
                rb.AddForce(-Physics2D.gravity * rb.gravityScale * flapTime * flapForce);
                if (!isInAttackRange && !isInViewRange)
                {
                    target = new Vector2(Random.Range(-1f, 1f), Random.Range(0, 1f));
                    rb.AddForce(target * Random.Range(speed_idle.x, speed_idle.y));
                    target -= new Vector2(0, Random.Range(0, 1f));
                }
            }
        }
    }
    private void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }
}
