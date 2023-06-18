using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DungeonInstancer))]
public class DungeonInstancer : MonoBehaviour
{
    public Vector2Int size;
    public int seed;
    public DungeonGenerator generator;
    public GridCamera cam;
    public GameObject RoomPrefab;
    public GameObject generateMenu;
    public TMP_InputField size_x_input;
    public TMP_InputField size_y_input;
    public TMP_InputField seed_input;
    public GameObject generateProgress;
    public Slider slider;
    public GameObject game;
    public CanvasGroup white;

    private void Start()
    {
        generator = GetComponent<DungeonGenerator>();
        game.SetActive(false);
        generateProgress.SetActive(false);
        generateMenu.SetActive(true);
        white.alpha = 1.0f;
        seed_input.text = Random.Range(0, int.MaxValue).ToString();
        Validate();
    }

    public void Validate()
    {
        int temp;
        int.TryParse(size_x_input.text, out temp);
        size.x = temp;
        size.x = Mathf.Max(size.x, 5);
        size_x_input.text = size.x.ToString();
        int.TryParse(size_y_input.text, out temp);
        size.y = temp;
        size.y = Mathf.Max(size.y, 3);
        size_y_input.text = size.y.ToString();
        int.TryParse(seed_input.text, out temp);
        seed = temp;
        seed_input.text = seed.ToString();
    }
    public void Generate()
    {
        Validate();
        StartCoroutine(Generate_Coroutine());
    }
    IEnumerator Generate_Coroutine()
    {
        generateProgress.SetActive(true);
        slider.maxValue = size.x * size.y + 1;
        generateMenu.SetActive(false);
        yield return null;
        Random.InitState(seed);
        generator.dungeonWidth = size.x;
        generator.dungeonHeight = size.y;
        generator.Generate();
        yield return null;
        slider.value++;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                InstantiateSala(new Vector2Int(x,y));
                yield return null;
                slider.value++;
            }
        }
        float time = 1;
        generateProgress.SetActive(false);
        game.SetActive(true);
        while(time > 0)
        {
            time -= Time.deltaTime / 0.5f;
            white.alpha = time;
        }
        white.alpha = 0;
    }

    public void InstantiateSala(Vector2Int pos)
    {
        Vector2 position = pos;
        position.x *= /*DungeonGenerator.roomWidth * */cam.size.x;
        position.y *= /*DungeonGenerator.roomHeight * */cam.size.y;
        Debug.Log(pos + ": " + position);
        DungeonRoom room = Instantiate(RoomPrefab, position, Quaternion.identity).GetComponent<DungeonRoom>();
        room.Spawn(pos, ref generator.dungeonMap);
    }
}
