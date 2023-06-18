using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonRoom : MonoBehaviour
{
    public Vector2Int pos;

    public GameObject[] backgrounds;

    public Transform[] positions0;
    public Transform[] positions1;
    public Transform[] positions2;
    public Transform[] positions3;

    public Transform getPos(int x, int y)
    {
        switch (y)
        {
            default:
                return positions3[x];
            case 1:
                return positions2[x];
            case 2:
                return positions1[x];
            case 3:
                return positions0[x];
        }
    }

    [System.Serializable]
    public struct Wall
    {
        public GameObject solid;
        public GameObject light;
    }
    public Wall[] Roof;
    public Wall[] Floor;
    public Wall[] Left;
    public Wall[] Right;

    public GameObject[] Enemies;

    public GameObject[] Obstacles;

    public GameObject StaticPlatform;
    public LinearMovement MovingPlatform;

    public GameObject UpgradeLife;
    public GameObject RegainLife;

    public Vector2 visibility = Vector2.one;
    public GridCamera gridCam;
    public LifeController playerLife;
    public GameObject objs;
    public void Spawn(Vector2Int pos, ref int[,,] dungeon)
    {
        gameObject.name = "RoomClone_" + pos.x + "_" + pos.y;
        this.pos = pos;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(false);
        }
        int room = (dungeon[this.pos.x * 9 + 1, this.pos.y * 5 + 1, 0]) % backgrounds.Length;
        backgrounds[room].SetActive(true);

        for (int x = 0;x < 8; x++)
        {
            if(dungeon[this.pos.x * 9 + 1 + x, this.pos.y * 5, 0] == 0)
            {
                Floor[x].solid.SetActive(false);
                Floor[x].light.SetActive(true);
            }
            else
            {
                Floor[x].solid.SetActive(true);
                Floor[x].light.SetActive(false);
            }
        }
        for (int x = 0; x < 8; x++)
        {
            if (dungeon[this.pos.x * 9 + 1 + x, this.pos.y * 5 + 5, 0] == 0)
            {
                Roof[x].solid.SetActive(false);
                Roof[x].light.SetActive(true);
            }
            else
            {
                Roof[x].solid.SetActive(true);
                Roof[x].light.SetActive(false);
            }
        }
        for (int y = 0; y < 4; y++)
        {
            if (dungeon[this.pos.x * 9, this.pos.y * 5 + 1 + y, 0] == 0)
            {
                Left[y].solid.SetActive(false);
                Left[y].light.SetActive(true);
            }
            else
            {
                Left[y].solid.SetActive(true);
                Left[y].light.SetActive(false);
            }
        }
        for (int y = 0; y < 4; y++)
        {
            if (dungeon[this.pos.x * 9 + 9, this.pos.y * 5 + 1 + y, 0] == 0)
            {
                Right[y].solid.SetActive(false);
                Right[y].light.SetActive(true);
            }
            else
            {
                Right[y].solid.SetActive(true);
                Right[y].light.SetActive(false);
            }
        }


        List<Transform> movingPoints = new List<Transform>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int xOffset = this.pos.x * 9 + 1 + x;
                int yOffset = this.pos.y * 5 + 1 + y;
                int value = 0;
                //Platforms
                value = dungeon[xOffset, yOffset, 1];
                if (value == 2)
                {
                    Transform spawnpos = getPos(x, y);
                    GameObject temp = Instantiate(StaticPlatform, spawnpos.position, Quaternion.identity);
                    temp.transform.parent = objs.transform;
                }
                else if (value == 3)
                {
                    movingPoints.Add(getPos(x, y));
                    movingPoints.Add(getPos(x, y));
                }
                //Enemies
                value = dungeon[xOffset, yOffset, 3];
                if (value > 0)
                {
                    Transform spawnpos = getPos(x, y);
                    value = (value - 1) % Enemies.Length;
                    GameObject temp = Instantiate(Enemies[value], spawnpos.position, Quaternion.identity);
                    temp.transform.parent = objs.transform;
                }
                //Obstacles
                value = dungeon[xOffset, yOffset, 2];
                if (value > 0)
                {
                    Transform spawnpos = getPos(x, y);
                    value = (value - 1) % Obstacles.Length;
                    GameObject temp = Instantiate(Obstacles[value], spawnpos.position, Quaternion.identity);
                    temp.transform.parent = objs.transform;
                }
                //Upgrades
                value = dungeon[xOffset, yOffset, 4];
                if (value == 1)
                {
                    Transform spawnpos = getPos(x, y);
                    CollisionEvent2D temp = Instantiate(UpgradeLife, spawnpos.position, Quaternion.identity).GetComponent<CollisionEvent2D>();
                    temp.transform.parent = objs.transform;
                    temp.onTriggerEnter.AddListener(delegate { Destroy(temp.gameObject); });
                    temp.onTriggerEnter.AddListener(delegate { playerLife.ModMaxLife(1); });
                }
                else if(value > 0)
                {
                    Transform spawnpos = getPos(x, y);
                    CollisionEvent2D temp = Instantiate(RegainLife, spawnpos.position, Quaternion.identity).GetComponent<CollisionEvent2D>();
                    temp.transform.parent = objs.transform;
                    temp.onTriggerEnter.AddListener(delegate { Destroy(temp.gameObject); });
                    temp.onTriggerEnter.AddListener(delegate { playerLife.ModLife(1); });
                }
            }
        }
        if(movingPoints.Count > 0)
        {
            MovingPlatform.destinations = new LinearMovement.Destination[movingPoints.Count];
            for (int i = 0; i < movingPoints.Count; i++)
            {
                MovingPlatform.destinations[i].target = movingPoints[i];
                int prevPos = i - 1;
                if(prevPos < 0) prevPos = movingPoints.Count - 1;
                MovingPlatform.destinations[i].duration = Mathf.Max(Vector3.Distance(movingPoints[i].position, movingPoints[prevPos].position) * 0.5f, 0.5f);
            }
        }
        else
        {
            MovingPlatform.gameObject.SetActive(false);
        }

        CalcCamera();
        objs.SetActive(onCamera);
    }
    private void Start()
    {
        gridCam = GameManager.scripts[GameManager.SCRIPTS.GridCamera] as GridCamera;
        playerLife = GameManager.scripts[GameManager.SCRIPTS.PlayerLifeController] as LifeController;
    }
    private void Update()
    {
        onCameraLast = onCamera;
        CalcCamera();
        if (onCameraLast != onCamera)
        {
            objs.SetActive(onCamera);
        }
    }

    public bool onCameraLast { get; protected set; } = false;
    public bool onCamera { get; protected set; } = false;
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
#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        DebugExtended.GizmosSaveColor();
        if (gridCam != null)
        {
            Gizmos.DrawWireCube((Vector2)gridCam.transform.position, gridCam.size);

            if (!Application.isPlaying)
            {
                CalcCamera();
            }
            if (onCamera)
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
