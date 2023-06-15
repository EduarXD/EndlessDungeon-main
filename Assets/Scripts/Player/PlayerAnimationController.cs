using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerMovement movement;
    [System.Serializable]
    public class TargetJoint2DManual
    {
        public TargetJoint2D joint;
        public Transform target;
        Vector3 lastPosition;
        public void Apply()
        {
            lastPosition = joint.transform.position; 
            joint.target = target.position; 
        }
        public void Reset() 
        {
            //Debug.Log(target.transform.position + " : " + lastPosition);
            joint.transform.position = lastPosition;
        }
    }
    public TargetJoint2DManual[] Joints;
    public SpriteRenderer sr;
    public Animator anim;
    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale  = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TargetJoint2DManual joint in Joints)
        {
            joint.Apply();
        }
        bool scaleChanged;
        if (movement.dir > 0 )
        {
            scaleChanged = transform.localScale.x > 0;
            transform.localScale = Vector3.Scale(originalScale, new Vector3(-1, 1, 1));
            anim.SetFloat("Horizontal", movement.rb.velocity.x);
        }
        else
        {
            scaleChanged = transform.localScale.x < 0;
            transform.localScale = originalScale;
            anim.SetFloat("Horizontal", movement.rb.velocity.x * -1f);
        }
        if (scaleChanged)
        {
            foreach (TargetJoint2DManual joint in Joints)
            {
                joint.Reset();
            }
        }
        anim.SetFloat("Vertical", movement.rb.velocity.y);
        anim.SetBool("Grounded", movement.gd.grounded);
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attack");
        }
    }
}
