using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public GameObject targetDoor;
    private GameObject player;
    private Vector3 pos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pos = targetDoor.transform.position;
        //pos.z = 0;
        print(this.name + " pos = " + pos);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.SendMessage("getTargetDoor", pos);
        }
    }
}
