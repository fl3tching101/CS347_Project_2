using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava_controller : MonoBehaviour
{

    public bool playerInLava; // If player is in the lava
    public int damageSpeed; // How quickly the player takes damage from lava
    public int damageLevel; // How much damage is done per damage cycle
    private int currentCountdown; // Current countdown before next damage
    private GameObject player; // Place to put the gameobject for the player 

    void Start()
    {
        currentCountdown = 0; // Start damage as soon as player touches lava
        player = GameObject.FindGameObjectWithTag("Player"); // Get the player for the scene
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if(playerInLava == true) // If player is in the lava
        {
            if (currentCountdown > 0)
            {
                currentCountdown--; // Countdown so that it hurts them in cycles
            }
            else
            {
                damagePlayer(); // Hurt the player
                currentCountdown = damageSpeed; // Reset countdown
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") // Detect player in lava
        {
            playerInLava = true; // Set the bool so the rest of the code knows the secret
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Detect player leaving lava
        {
            playerInLava = false;
            currentCountdown = 0; // Reset the current damage countdown
        }
    }

    void damagePlayer()
    {
        player.SendMessage("applyDamage", damageLevel); // Let the player know the lava wants them to die a bit more
    }
}
