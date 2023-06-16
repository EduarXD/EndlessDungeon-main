using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(DungeonGenerator))]
public class DungeonPreviewer : MonoBehaviour
{
    public DungeonGenerator dungeon;
    public GridCamera grid;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        PreView();
    }

    public void PreView()
    {
        DebugExtended.GizmosSaveColor();
        if (dungeon)
        {
            if (dungeon.dungeonMap == null)
            {
                dungeon.Generate();
            }
            try
            {
                if (grid)
                {
                    Debug.Log("Recargando visualización");
                    //Salas
                    for (int x = 0; x < dungeon.dungeonWidth * 9 + 1; x++)
                    {
                        for (int y = 0; y < dungeon.dungeonHeight * 5 + 1; y++)
                        {
                            switch (dungeon.dungeonMap[x, y, 0])
                            {
                                case 1:
                                    Gizmos.color = Color.black;
                                    break;
                                case 2:
                                    Gizmos.color = Color.blue;
                                    break;
                                case 3:
                                    Gizmos.color = Color.red;
                                    break;
                                case 4:
                                    Gizmos.color = Color.green;
                                    break;
                                default:
                                    Gizmos.color = Color.white;
                                    break;
                            }
                            Gizmos.DrawWireCube(new Vector3(x, y), Vector2.one);
                            Gizmos.color = Gizmos.color * new Color(1, 1, 1, 0.25f);
                            Gizmos.DrawCube(new Vector3(x, y), Vector2.one);
                        }
                    }
                    bool lack = true;
                    //Plataformas
                    for (int x = 0; x < dungeon.dungeonWidth * 9 + 1; x++)
                    {
                        for (int y = 0; y < dungeon.dungeonHeight * 5 + 1; y++)
                        {
                            if (dungeon.dungeonMap[x, y, 1] == 2)
                            {
                                Gizmos.color = Color.white;
                                Gizmos.DrawSphere(new Vector3(x, y, -2), 0.25f);
                                lack = false;
                            }
                            if (dungeon.dungeonMap[x, y, 1] == 3)
                            {

                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(new Vector3(x, y, -2), 0.25f);
                                lack = false;
                            }

                        }
                    }
                    if (lack)
                        Debug.LogWarning("No se han encontrado plataformas");
                    lack = true;
                    //Obstaculos
                    for (int x = 0; x < dungeon.dungeonWidth * 9 + 1; x++)
                    {
                        for (int y = 0; y < dungeon.dungeonHeight * 5 + 1; y++)
                        {
                            if (dungeon.dungeonMap[x, y, 2] > 0)
                            {
                                if (dungeon.dungeonMap[x, y, 2] < 5)
                                {
                                    Gizmos.color = Color.black;
                                }
                                else
                                {
                                    Gizmos.color = Color.cyan;
                                }
                                Gizmos.DrawSphere(new Vector3(x, y, -2), 0.25f);
                                lack = false;
                            }
                        }
                    }
                    if (lack)
                        Debug.LogWarning("No se han encontrado obstaculos");
                    lack = true;
                    //Enemigos
                    for (int x = 0; x < dungeon.dungeonWidth * 9 + 1; x++)
                    {
                        for (int y = 0; y < dungeon.dungeonHeight * 5 + 1; y++)
                        {
                            if (dungeon.dungeonMap[x, y, 3] > 0)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(new Vector3(x, y, -3), 0.25f);
                                lack = false;
                            }
                        }
                    }
                    if (lack)
                        Debug.LogWarning("No se han encontrado enemigos");
                    lack = true;
                    //Power ups
                    for (int x = 0; x < dungeon.dungeonWidth * 9 + 1; x++)
                    {
                        for (int y = 0; y < dungeon.dungeonHeight * 5 + 1; y++)
                        {
                            if (dungeon.dungeonMap[x, y, 4] > 0)
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawSphere(new Vector3(x, y, -4), 0.25f);
                                lack = false;
                            }
                        }
                    }
                    if (lack)
                        Debug.LogWarning("No se han encontrado power ups");

                }
            }
            catch { }
        }
        DebugExtended.GizmosRestoreColor();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonPreviewer))]
public class DungeonPreviewerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myScript = target as DungeonPreviewer;
        if (GUILayout.Button("Generate"))
        {
            myScript.dungeon.Generate();
            myScript.PreView();
        }
    }
}
#endif