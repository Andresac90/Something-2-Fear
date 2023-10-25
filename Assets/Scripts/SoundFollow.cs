using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFollow : MonoBehaviour
{
    private static SoundFollow instance = null;

    public static SoundFollow Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
