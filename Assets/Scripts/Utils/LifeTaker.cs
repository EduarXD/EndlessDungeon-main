using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionEvent2D))]
public class LifeTaker : MonoBehaviour
{
    public CollisionEvent2D collisionEvent;

    public int damage;
    public float knockbackMultiply;
    public float knockbackAdd;
    public float pushup;
    public bool ignoreInvencible;
    public float recover;
    float nextHit;

    // Start is called before the first frame update
    void Start()
    {
        collisionEvent = GetComponent<CollisionEvent2D>();
        collisionEvent.onCollisionEnter.AddListener(Damage);
        collisionEvent.onTriggerEnter.AddListener(Damage);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(Collision2D collision)
    {
        Damage(collision.gameObject);
    }
    public void Damage(Collider2D collider)
    {
        Damage(collider.gameObject);
    }
    public void Damage(GameObject obj)
    {
        if (nextHit > Time.time) return;
        LifeController life = obj.GetComponentInChildren<LifeController>();
        if(life != null)
        {
            nextHit = Time.time + recover;
            life.Damage(damage, knockbackMultiply, obj.transform.position.x < transform.position.x ? -knockbackAdd : knockbackAdd, pushup, ignoreInvencible);
        }
    }
}
