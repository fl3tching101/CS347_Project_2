using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour
{
    public bool facingRight;                                        // Am I facing right? Fancy
    public float speed;                                             // Rate at which velocity is added
    public int health;                                              // How much life you have left til u DED
    public bool isJumping;                                          // Am I jumping right now? Huh, that's weird
    public float jumpForce;                                         // Force with which to jump
    public int numJumps;                                            // Number of jumps allowed
    public float maxSpeed;                                          // Maximum speed allowed
    public Image health_image_1, health_image_2, health_image_3;    // Health images from UI
    public Sprite emptyHealth, halfHealth, fullHealth;              // Health sprites
    private Rigidbody2D rb;                                         // The body of rigid
    private Vector2 direction;                                      // Which way should I go? Different from facing right because this is how I acquire the movement direction
    private bool startJump;                                         // Start jumping... NOW!
    private int jumpsLeft;                                          // Current number of jumps remaining
    private SpriteRenderer spr;                                     // The renderer of the Sprite (Copyright Coca-Cola company, all rights reserved)
    private Animator anim;                                          // I've got the moves like... my animator
    private bool doorUsable;                                        // If standing in door
    private Vector3 targetDoorPos;                                  // Position of the targeted door
    private bool isPaused;                                          // Used to pause the game
    private Text scoreText;                                         // Text that the final score is saved into
    private AudioSource deathSource;                                // Source for death noise
    public AudioClip deathSound;                                    // Sound effect for dying

    private enum state_type {idle, moving, jumping, sliding}        // enumerated type for the state the animation should be in
    state_type cur_state;                                           // Variable to hold the enum


    void Awake()
    {
        QualitySettings.vSyncCount = 1; // I mean, I'm all about getting more FPS... but 2,900+ FPS was a bit much...
    }

    void Start() // How everyhing starts up
    {
        facingRight = true;                         // Start facing right... cuase why not?
        isJumping = false;                          // Don't start by jumping... that's just weird
        rb = GetComponent<Rigidbody2D>();           // Grab yo body
        spr = GetComponent<SpriteRenderer>();       // Grab a can of... your sprite renderer
        jumpsLeft = numJumps;                       // Set up number of jumps
        cur_state = state_type.idle;                // Start at idle... cause why not?
        anim = GetComponent<Animator>();            // Get your animator
        doorUsable = false;                         // Can I open it yet??? Can I?!?!?
        targetDoorPos = Vector3.zero;               // Start the targeted door at <0,0,0>... no real reason, but better than null
        isPaused = false;                           // Should everything stop? I feel like everything should stop...
        deathSource = GetComponent<AudioSource>();  // This is the werid component I need in order to die... probably should have left that out, but alas I suppose we all die eventually...
    }

    void Update() // Runs a lot, especially without that awake function... yeah, use vsync... you'll regret not doing that
    {
        if (isPaused != true) // If I'm not paused
        {
            Time.timeScale = 1.0f;                                              // Time should move as usual... 
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);      // I should go in the direction the player wants me to... though randomly ignoring it or doing the opposite would be pretty fun
            if (direction.magnitude > 0.0f)                                     // Which way was I going again?
            {
                if (direction.x > 0.0f)                                         // Right!
                {
                    facingRight = true;
                }
                else                                                            // Wait no... left?
                {
                    facingRight = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))                                // I should probably jump, space usually means jump. Unless you are typing, then definitely don't jump
            {
                startJump = true;                                               // I knew it, I shuold jump
                cur_state = state_type.jumping;                                 // Currently my animator should be jumping
            }
            switch (cur_state)                                                  // Decide what my character should appear to be doing right now... only mostly correlates with what he's actually doing though...
            {
                case (state_type.idle):                                         // Just standing there? Really? Is that what you call fun?
                    anim.SetInteger("state", 0);                                // Let the animator know I'm lazy...
                    break;
                case (state_type.moving):                                       // Now we're talking!
                    anim.SetInteger("state", 1);                                // Let the animator know I'm going for a run
                    break;
                case (state_type.jumping):                                      // One small step for a man... one giant leap for a Sprite (Copyright Coca-Cola... haha not funny anymore)
                    anim.SetInteger("state", 2);                                // Let the animator know I'm leaping for joy... or death
                    break;
                case (state_type.sliding):                                      // I'm falling... with style!
                    anim.SetInteger("state", 3);                                // Animator, I'm going to infinity, and beyoooonnnndddd!!!
                    break;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (doorUsable) // May I come in?
                {
                    //rb.MovePosition(targetDoorPos); // Yeah, this was a mistake, not really sure why it breaks, but it sure does
                    transform.SetPositionAndRotation(targetDoorPos, transform.rotation); // Move the transform, not the rigidbody... you'll regret not doing this one
                }
            }
            if (rb.velocity.magnitude > 0.1f && !(isJumping) && !(cur_state == state_type.sliding)) // If I'm moving, and I'm not jumping, and I'm not fall.. I mean sliding, then I guess I'm running
            {
                cur_state = state_type.moving;      // Set current movement to run mode
            }
            else if (rb.velocity.magnitude < 0.1f && !(isJumping) && !(cur_state == state_type.sliding))    // Yeah... just sitting there... useless
            {
                cur_state = state_type.idle; // May as well just quit the game if you're just going to stand here, but I better set the state to it or you'll look like you're actually doing something
            }
        }
        else // Need to stop physics too
        {
            Time.timeScale = 0.0f; // Stop!... Wait a minute.
        }
    }

    void FixedUpdate() // This one runs a set amount of times per second... physics stuff goes here
    {
        if(isPaused != true)
        {
            //rb.velocity = new Vector2(direction.x * speed, rb.velocity.y); // Sudo physics based controller
            if (direction.magnitude > 0.0f) // physics based controller
            {
                if (facingRight == true) // I keep forgetting, which way am I facing?
                {
                    spr.flipX = false; // Don't flip the Sprite... bottle flip challenges are meant for water
                    if (rb.velocity.x < maxSpeed) // Prevent player from going too fast. Turns out this is kinda important
                    {
                        rb.AddForce(Vector2.right * speed); // Slide to the right
                    }
                }
                else
                {
                    spr.flipX = true;
                    if (rb.velocity.x > -1 * maxSpeed) // Prevent player from going too fast
                    {
                        rb.AddForce(Vector2.left * speed); // Slide to the left
                    }
                }
            }
            if (startJump == true) // Criss cross!... wait no, jump!
            {
                jump(); // JUMP! DO IT NOW!
                startJump = false; // I just did that, don't do it again, could be bad
            }
        }
    }

    void jump() // JUMP!!!! DOOOOOOOO ITTTTT!!!
    {
        if(jumpsLeft > 0) // But like, only if you are allowed to
        {
            rb.AddForce(Vector2.up * jumpForce); // Add force upwards. No need to limit since the number of jumps is limited
            isJumping = true; // Yep, can confirm, I am jumping
            jumpsLeft--; // But I can only do it twice, so better keep count
        }
    }

    void applyDamage(int damageAmount) // Ouch
    {
        health -= damageAmount; // There goes... damageAmount of my life
        
        switch (health) // Display how much health is left, might be a better way of doing this, but I sure don't know it
        {
            case 5:
                health_image_3.sprite = halfHealth; // 2.5 hearts
                break;
            case 4:
                health_image_3.sprite = emptyHealth; // 2 hearts
                break;
            case 3:
                health_image_2.sprite = halfHealth; // 1.5 hearts
                break;
            case 2:
                health_image_2.sprite = emptyHealth; // 1 hearts
                break;
            case 1:
                health_image_1.sprite = halfHealth; // 0.5 hearts ... You might want to consider your life choices at this point
                break;
            case 0: // Ded - D E D ded
                health_image_1.sprite = emptyHealth; // 0 hearts... See, I told you, but you went and did it anyway
                StartCoroutine(playDeathSound()); // Oof... also reload the scene, I know I can get it next time!
                //deathSource.PlayOneShot(deathSound);     // Yeah this didn't work. Got cut off by the end of the scene happening like 20 nanoseconds later           
                break;

        }
    }

    void OnCollisionEnter2D(Collision2D collision) // I'm inside of something, I can feel it
    {
        if(collision.gameObject.tag == "Ceiling") // Was that the ceiling? 
        {
            jumpsLeft = numJumps; // Reset number of jumps
            cur_state = state_type.sliding; // Sliding down
            isJumping = false; // Not jumping anymore
        }else if(collision.gameObject.tag == "Floor") // Nope, that was the floor. Floor, ceiling, same thing
        {
            jumpsLeft = numJumps; // Reset number of jumps
            cur_state = state_type.idle; // Back to doin nothing I suppose
            isJumping = false; // Not jumping anymore
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // No longer in something
    {
        if (collision.gameObject.tag == "Ceiling") // No longer in the ceiling
        {            
            cur_state = state_type.jumping; // Probably falling... this is where the animations get a little questionable
            isJumping = true; // Again, probably falling, which is basically jumping
        }
        else if (collision.gameObject.tag == "Floor") // No longer on the floor
        {
            cur_state = state_type.jumping; // Again, probably jumping? Hard to tell
            isJumping = true; // Jumping
        }
    }

    void OnTriggerEnter2D(Collider2D other) // In something again, feels less painful though
    {
        if(other.tag == "Door") // In a door
        {
            doorUsable = true;  // Now I can open the door!
        }        
    }
    void OnTriggerExit2D(Collider2D collision) // Leaving the door
    {
        if (collision.tag == "Door") // Yep, door go bye bye
        {
            doorUsable = false;     // Can't open it anymore
        }
    }

    void getTargetDoor(Vector3 pos) // Grab the positon of the door I'm going to
    {
        targetDoorPos = pos; // Yep, that position. Well, hopefully
    }

    void pauseGame() // Hold everything!!! Not as easy as you'd think actually...
    {
        isPaused = true; // Should be paused now
    }

    void playGame() // Ok, now go!
    {
        isPaused = false; // Not paused, go, go, go!
    }

    void endOfLevel() // Is it over? Please let it be over!
    {
        isPaused = true; // Stop the game while we get your name and put your score on the highscores... if you are good enough
        GameObject.Find("Player/UI/playerNameUI").SetActive(true); // Get the name of this fine player
    }

    IEnumerator playDeathSound() // Weird coroutine thing? The internet says this is how you can play a sound and wait for it to end before doing something else
    {        
        deathSource.PlayOneShot(deathSound); // You are DED
        yield return new WaitWhile(()=> deathSource.isPlaying); // Wait for the screams of dying to end... the terrible screams...
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart!
    }
}
