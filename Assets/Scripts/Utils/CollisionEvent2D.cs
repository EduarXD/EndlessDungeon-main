using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CollisionEvent2D : MonoBehaviour
{
    public bool destroyOnTrigger;
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
        if (this.enabled || gameObject.activeInHierarchy)
            if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collision.gameObject.layer))) && (tag_ignore || tag_compare == collision.gameObject.tag))
            {
                onCollisionEnter.Invoke(collision);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                    this.enabled = false;
                }
            }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (this.enabled || gameObject.activeInHierarchy)
            if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collision.gameObject.layer))) && (tag_ignore || tag_compare == collision.gameObject.tag))
            {
                onCollisionStay.Invoke(collision);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                    this.enabled = false;
                }
            }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (this.enabled || gameObject.activeInHierarchy)
            if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collision.gameObject.layer))) && (tag_ignore || tag_compare == collision.gameObject.tag))
            {
                onCollisionExit.Invoke(collision);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                    this.enabled = false;
                }
            }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.enabled || gameObject.activeInHierarchy)
            if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collider.gameObject.layer))) && (tag_ignore || tag_compare == collider.gameObject.tag))
            {
                onTriggerEnter.Invoke(collider);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                    this.enabled = false;
                }
            }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (this.enabled || gameObject.activeInHierarchy)
            if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collider.gameObject.layer))) && (tag_ignore || tag_compare == collider.gameObject.tag))
            {
                onTriggerStay.Invoke(collider);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                    this.enabled = false;
                }
            }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (this.enabled || gameObject.activeInHierarchy)
            if ((mask_ignore || mask_compare.value == (mask_compare.value | (1 << collider.gameObject.layer))) && (tag_ignore || tag_compare == collider.gameObject.tag))
            {
                onTriggerExit.Invoke(collider);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                    this.enabled = false;
                }
            }
    }
}
