﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour
{
    public bool facingRight;
    public float speed;
    public int health;
    public bool isJumping;
    public float jumpForce;
    public int numJumps; // Number of jumps allowed
    public float maxSpeed; // Maximum speed allowed
    public Image health_image_1, health_image_2, health_image_3; // Health images from UI
    public Sprite emptyHealth, halfHealth, fullHealth; // Health sprites
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool startJump;
    private int jumpsLeft; // Current number of jumps remaining
    private SpriteRenderer spr;
    private Animator anim;
    private bool doorUsable; // If standing in door
    private Vector3 targetDoorPos; // Position of the targeted door

    private enum state_type {idle, moving, jumping, sliding}
    state_type cur_state;

    void Start()
    {
        facingRight = true;
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        jumpsLeft = numJumps;
        cur_state = state_type.idle;
        anim = GetComponent<Animator>();
        doorUsable = false;
        targetDoorPos = Vector3.zero;
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);
        if (direction.magnitude > 0.0f)
        {
            if (!isJumping && !(cur_state == state_type.sliding))
            {
                cur_state = state_type.moving;
            }
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
            cur_state = state_type.jumping;
        }
        switch (cur_state)
        {
            case (state_type.idle):
                    anim.SetInteger("state", 0);
                    break;
            case (state_type.moving):
                    anim.SetInteger("state", 1);
                    break;                
            case (state_type.jumping):
                    anim.SetInteger("state", 2);
                    break;                
            case (state_type.sliding):
                    anim.SetInteger("state", 3);
                    break;                
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (doorUsable)
            {
                rb.MovePosition(targetDoorPos);
            }
        }
    }

    void FixedUpdate()
    {
        //rb.velocity = new Vector2(direction.x * speed, rb.velocity.y); // Sudo physics based controller
        if (direction.magnitude > 0.0f) // physics based controller
        {
            if (facingRight == true)
            {
                spr.flipX = false;
                if(rb.velocity.x < maxSpeed) // Prevent player from going too fast
                {
                    rb.AddForce(Vector2.right * speed);
                }
            }
            else
            {
                spr.flipX = true;
                if (rb.velocity.x > -1* maxSpeed) // Prevent player from going too fast
                {
                    rb.AddForce(Vector2.left * speed);
                }
            }
        }
        if (startJump == true)
        {
            jump();
            startJump = false;
        }
    }

    void jump()
    {
        if(jumpsLeft > 0)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpsLeft--;
        }
    }

    void applyDamage(int damageAmount)
    {
        health -= damageAmount;
        
        switch (health)
        {
            case 5:
                health_image_3.sprite = halfHealth;
                break;
            case 4:
                health_image_3.sprite = emptyHealth;
                break;
            case 3:
                health_image_2.sprite = halfHealth;
                break;
            case 2:
                health_image_2.sprite = emptyHealth;
                break;
            case 1:
                health_image_1.sprite = halfHealth;
                break;
            case 0:
                health_image_1.sprite = emptyHealth;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ceiling")
        {
            jumpsLeft = numJumps; // Reset number of jumps
            cur_state = state_type.sliding;
        }else if(collision.gameObject.tag == "Floor")
        {
            jumpsLeft = numJumps; // Reset number of jumps
            cur_state = state_type.idle;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Door")
        {
            doorUsable = true;            
        }        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            doorUsable = false;            
        }
    }

    void getTargetDoor(Vector3 pos)
    {
        targetDoorPos = pos;
    }
}
