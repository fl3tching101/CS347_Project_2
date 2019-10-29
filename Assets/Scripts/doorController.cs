using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public GameObject targetDoor; // The target door, or any object really, for the teleport
    private GameObject player; // Player to teleport
    private Vector3 pos; // Position to send them to

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player
        pos = targetDoor.transform.position;        // Move the player to the target door
        //print(this.name + " pos = " + pos);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") // If the player is in a door, send the coordinates of the door
        {
            player.SendMessage("getTargetDoor", pos); // Send coordinates
        }
    }
}
