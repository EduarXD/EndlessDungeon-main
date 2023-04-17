
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GroundDetector : MonoBehaviour
{
    public bool grounded;
    [SerializeField]
    private float groundDistance = 1.5f;
    public List<Vector3> rays;
    public LayerMask groundMask;
    public Transform mobilePlatform;

    void OnValidate()
    {
        CheckGround();
    }

    void Update()
    {
        CheckGround();
    }
    public void CheckGround()
    {
        int count = 0;
        mobilePlatform = null;
        for (int i = 0; i < rays.Count; i++)
        {
            Debug.DrawRay(transform.position + rays[i], transform.up * -1 * groundDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + rays[i], transform.up * -1, groundDistance, groundMask);
            if (hit.collider != null)
            {
                count++;
                Debug.DrawRay(transform.position + rays[i], transform.up * -1 * hit.distance, Color.green);
                if (hit.transform.tag == "PlataformaMovil")
                {
                    mobilePlatform = hit.transform;
                }
            }
        }
        if (Application.isPlaying)
        {
            transform.parent = mobilePlatform;
        }
        if (count > 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
            if (Application.isPlaying)
            {
                transform.parent = null;
            }
        }
    }
}