using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{

    private GameObject player;
    private bool curPaused; // Currently paused?
    public GameObject menu; // gameobject for menu itself

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        curPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Hit escape");
            if(curPaused == false) // Currently running, pause game
            {
                print("Paused");
                player.SendMessage("pauseGame");
                curPaused = true;
                menu.SetActive(true);
            }
            else // Currently paused, resume
            {
                print("Play");
                player.SendMessage("playGame");
                curPaused = false;
                menu.SetActive(false);
            }
        }
    }
}
