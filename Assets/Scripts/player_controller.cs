using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public bool facingRight;
    public float speed;
    public int health;
    public bool isJumping;
    public float jumpForce;
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool startJump;
    
    void Start()
    {
        facingRight = true;
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);
        if (direction.magnitude > 0.0f)
        {
            if (direction.x > 0.0f)
            {
                facingRight = true;
            }
            else
            {
                facingRight = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startJump = true;
        }
    }

    void FixedUpdate()
    {
        if(direction.magnitude > 0.0f)
        {
            if(facingRight == true)
            {
                rb.AddForce(Vector2.right * speed);
            }
            else
            {
                rb.AddForce(Vector2.left * speed);
            }
        }
        if(startJump == true)
        {
            jump();
            startJump = false;
        }
    }

    void jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
    }
}
