using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Collider2D))]
public class CollisionEvent2D : MonoBehaviour
{
    public bool tag_ignore = true;
    public string tag_compare;
    public bool mask_ignore = true;
    public LayerMask mask_compare;
    public UnityEvent<Collision2D> onCollisionEnter;
    public UnityEvent<Collision2D> onCollisionExit;
    public UnityEvent<Collision2D> onCollisionStay;
    public UnityEvent<Collider2D> onTriggerEnter;
    public UnityEvent<Collider2D> onTriggerExit;
    public UnityEvent<Collider2D> onTriggerStay;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collision.gameObject.layer))) && (tag_ignore || tag_compare == collision.gameObject.tag))
        {
            onCollisionEnter.Invoke(collision);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collision.gameObject.layer))) && (tag_ignore || tag_compare == collision.gameObject.tag))
        {
            onCollisionStay.Invoke(collision);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collision.gameObject.layer))) && (tag_ignore || tag_compare == collision.gameObject.tag))
        {
            onCollisionExit.Invoke(collision);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collider.gameObject.layer))) && (tag_ignore || tag_compare == collider.gameObject.tag))
        {
            onTriggerEnter.Invoke(collider);
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collider.gameObject.layer))) && (tag_ignore || tag_compare == collider.gameObject.tag))
        {
            onTriggerStay.Invoke(collider);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collider.gameObject.layer))) && (tag_ignore || tag_compare == collider.gameObject.tag))
        {
            onTriggerExit.Invoke(collider);
        }
    }              
}
