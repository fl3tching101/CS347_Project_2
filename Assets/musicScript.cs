using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicScript : MonoBehaviour
{
    public static musicScript musicData;  // This stuff makes it a singleton class, so the music don't stop!
    void Awake()
    {
        if (musicData == null) 
        {
            DontDestroyOnLoad(gameObject);
            musicData = this;
        }
        else if (musicData != this)
        {
            Destroy(gameObject);
        }
    }
}
