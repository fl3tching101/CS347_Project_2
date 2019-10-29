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
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player
        curPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //print("Hit escape");
            if(curPaused == false) // Currently running, pause game
            {
                //print("Paused");
                player.SendMessage("pauseGame"); // Let the player know to pause all of his actions
                curPaused = true; // Bool for the if statements
                menu.SetActive(true); // Make the menu active
            }
            else // Currently paused, resume
            {
                //print("Play");
                player.SendMessage("playGame"); // Let the player know to start playing again
                curPaused = false;
                menu.SetActive(false); // Hide menu
            }
        }
    }
}
