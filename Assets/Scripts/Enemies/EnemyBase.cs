using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public AttackPatterns attacks;
    public GridCamera gridCam;
    public Vector2 visibility = Vector2.one;
    public LifeController player;
    public LifeController life;
    public bool isVisible
    {
        get
        {
            if(gridCam != null)
            {
                Bounds cameraBounds = new Bounds((Vector2)gridCam.transform.position, gridCam.size);
                Bounds visibilityBounds = new Bounds((Vector2)transform.position, visibility);
                return cameraBounds.Intersects(visibilityBounds);
            }
            else { return false; }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gridCam = GameManager.scripts[GameManager.SCRIPTS.GridCamera] as GridCamera;
        player = GameManager.scripts[GameManager.SCRIPTS.PlayerLifeController] as LifeController;
    }

    // Update is called once per frame
    void Update()
    {

    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        DebugExtended.GizmosSaveColor();
        if (gridCam != null)
        {
            Gizmos.DrawWireCube((Vector2)gridCam.transform.position, gridCam.size);
            if(isVisible)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireCube((Vector2)transform.position, visibility);
        }
        DebugExtended.GizmosRestoreColor();
    }
#endif
}
