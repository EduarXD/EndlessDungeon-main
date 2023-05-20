using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public TMP_FontAsset font_default;
    public TMP_FontAsset font_dyslexic;
    public bool use_dyslexic;
    public EventSystem eventSystem;
    public CanvasScaler canvasScaler;
    Vector2 canvasScalerSize;
    [System.Serializable]
    public struct SubMenu
    {
        public GameObject menu;
        public UnityEngine.UI.Selectable defaultItem;
    }
    public uint current;
    public SubMenu[] subMenus;
    private void Start()
    {
        CheckFont();
        for (int i = 0; i < subMenus.Length; i++)
        {
            subMenus[i].menu.SetActive(false);
        }
        ChangeMenu((int)current);
        canvasScalerSize = canvasScaler.referenceResolution;
    }
    public void ChangeMenu(int index)
    {
        subMenus[current].menu.SetActive(false);
        current = (uint)Mathf.Clamp(index, 0, subMenus.Length - 1);
        subMenus[current].menu.SetActive(true);
        subMenus[current].defaultItem.Select();
        eventSystem.firstSelectedGameObject = subMenus[current].defaultItem.gameObject;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ChangeDyslexic(bool use_dyslexic) { this.use_dyslexic = use_dyslexic; CheckFont(); }
    public void ChangeFullscreen(bool fullscreen) { Screen.fullScreen = fullscreen; }
    public void CheckFont()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
        if (use_dyslexic)
        {
            foreach (TMP_Text text in texts)
            {
                text.font = font_dyslexic;
            }
        }
        else
        {
            foreach (TMP_Text text in texts)
            {
                text.font = font_default;
            }
        }
    }
    public void ChangeUIScale(string scaletext)
    {
        float scale = 0;
        if(!float.TryParse(scaletext, out scale))
        {
            scale = 1;
        }
        scale = Mathf.Clamp(scale, 0.75f, 2f);
        canvasScaler.referenceResolution = canvasScalerSize * scale;
    }
}
