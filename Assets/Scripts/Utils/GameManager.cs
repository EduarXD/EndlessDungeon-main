using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject Player;
    public enum SCRIPTS { PlayerMovement, PlayerLifeController, UIController, GridCamera }
    public static Dictionary<SCRIPTS, MonoBehaviour> scripts = new Dictionary<SCRIPTS, MonoBehaviour>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
