using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    
    [Min(-1)]
    public int fpsMax = 30000;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        Instance = this;
    }

    void LateUpdate()
    {
        Application.targetFrameRate = fpsMax;
    }
}
