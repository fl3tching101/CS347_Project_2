using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    private GameObject player;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find player
    }

    void LateUpdate()
    {
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax); // Set the maximum and minimum x values of the camera
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax); // Set the maximum and minimum y values of the camera
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z); // Move the camera to the new location
    }
}
