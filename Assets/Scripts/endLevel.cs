using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endLevel : MonoBehaviour
{
    private GameObject player; // Player's game object

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player's game object
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") // If the player has reached the end level object
        {
            player.SendMessage("endOfLevel"); // Tell the player they are done and can end the level
        }
    }
}
