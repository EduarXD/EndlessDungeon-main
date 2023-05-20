using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [System.Serializable]
    public struct Life
    {
        public Transform parent;
        [System.Serializable]
        public struct Display
        {
            public GameObject obj;
            public Image foreground;
        }
        public Display original;
        public List<Display> displays;
        public void DuplicateDisplay()
        {
            Display display = new Display();
            display.obj = Instantiate(original.obj, parent);
            display.foreground = display.obj.transform.Find(original.foreground.transform.name).GetComponent<Image>();
            display.obj.SetActive(true);
            displays.Add(display);
        }
        public void DeleteDisplay()
        {
            Destroy(displays[displays.Count - 1].obj);
            displays.RemoveAt(displays.Count - 1);
        }
    }
    public Life lifeController;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.scripts.Add(GameManager.SCRIPTS.UIController, this);
    }

    // Update is called once per frame
    void Update()
    {
        LifeController playerLife = GameManager.scripts[GameManager.SCRIPTS.PlayerLifeController] as LifeController;
        while (Mathf.Max(playerLife.maxLifes, playerLife.lifes) > lifeController.displays.Count)
        {
            lifeController.DuplicateDisplay();
        }
        while (Mathf.Max(playerLife.maxLifes, playerLife.lifes) < lifeController.displays.Count && lifeController.displays.Count > 1)
        {
            lifeController.DeleteDisplay();
        }
        for (int i = lifeController.displays.Count - 1; i >= 0; i--)
        {
            if (i < playerLife.lifes)
            {
                lifeController.displays[i].foreground.enabled = true;
            }
            else
            {
                lifeController.displays[i].foreground.enabled = false;
            }
        }
    }
}
