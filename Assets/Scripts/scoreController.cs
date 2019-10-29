using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreController : MonoBehaviour
{
    private float timer; // The current time since start of game
    private bool time_run; // Whether the timer should be running
    public Text timer_UI; // Where the timer is displayed
    public int minTime; // Minimum time to complete level (minimum time to play through level for developer divided by two or so for skill ceiling)
    public int failTime; // Time beyond which score will be base, no time bonus
    public int baseScore; // Base score for level... kind of silly, but at least if they get no time bonus they don't get 0 score
    public string playerName; // The name of the player, found at the end of level
    private int coins_collected; // Number of coins collected
    public int levelNum;            // 0-2, cause counting starts at 0. Used for the highscore table
    public AudioSource coinTink; // Source for the coin tink when picking up coins
    public AudioClip coinSound; // Actual audio

    void Start()
    {
        timer = 0.0f; // Initialize the timer to 0
        time_run = true; // Start the timer
        playerName = ""; // Name of the player
        coins_collected = 0; // How many coins collected so far
    }

    void Update()
    {
        if(time_run == true)
        {
            timer += Time.deltaTime; // Run timer
        }
    }

    void FixedUpdate()
    {
        timer_UI.text = (""+(int)timer); // Update timer in UI
    }

    void stopTimer()
    {
        time_run = false; // Stop timer
    }

    public int calculateScore()
    {
        stopTimer(); // Stop the timer, game is over
        int score = 0; // Start with a score of 0
        int health = GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller>().health; // Get how much health is left
        if(timer < minTime) // Make sure time doesn't go negative when minTime is subtracted off their time
        {
            minTime = (int)timer;
        }
        int time_score_func = failTime - ((int)timer - minTime); // Formula to make longer times worse
        if(time_score_func < 0) // If they are over failTime, then just give them a time bonus of 0
        {
            time_score_func = 0; 
        }
        score = baseScore + (health * time_score_func) + 100*coins_collected; // Formula I came up with to calculate score, kind of random, but it works and it seems to be pretty good
        handleHighScore(score); // Take care of the highscore table
        return score; // Return the calculated score
    }

    private void handleHighScore(int score) // Determine if the score is good enough, and if it is, put it into the highscores
    {
        int prevBest = 0, curBest = 0; // Temporary variables for when we are swapping around things
        string prevPlayer = "", curPlayer = ""; // Same as above
        bool foundPlace = false; // If a place in the highscores has been found or not

        dataController.dataManagement.loadData(); // Load saved data
        //for (int i = 0; i < 10; i++)
        //{
        //    print("ScoreController Score for place " + i + " : " + dataController.dataManagement.highscores_tmp[i]);
        //    print("ScoreController Player for place " + i + " : " + dataController.dataManagement.highscore_player_tmp[i]);
        //}

        for (int i = 0; i < 10; i++){ // Loop through the highscore data and see if the current score made it
            if (foundPlace) // Runs on loop after place is found
            { // Swaps the previous higher score with the next lower score, place 10 (or 9 since starts at 0) falls off list
                curBest = dataController.dataManagement.highscores_tmp[i,levelNum];
                curPlayer = dataController.dataManagement.highscore_player_tmp[i, levelNum];
                dataController.dataManagement.highscores_tmp[i, levelNum] = prevBest;
                dataController.dataManagement.highscore_player_tmp[i, levelNum] = prevPlayer;
                prevBest = curBest;
                prevPlayer = curPlayer;
            }
            if (score >= dataController.dataManagement.highscores_tmp[i, levelNum] && foundPlace == false) // Until we find the place for the current score, loop forever
            {
                foundPlace = true; // Found it
                prevBest = dataController.dataManagement.highscores_tmp[i, levelNum];
                prevPlayer = dataController.dataManagement.highscore_player_tmp[i, levelNum];
                dataController.dataManagement.highscores_tmp[i, levelNum] = score;
                dataController.dataManagement.highscore_player_tmp[i, levelNum] = playerName;
            }            
        }
        //for (int i = 0; i < 10; i++)
        //{
        //    print("ScoreController Score for place " + i + " : " + dataController.dataManagement.highscores_tmp[i]);
        //    print("ScoreController Player for place " + i + " : " + dataController.dataManagement.highscore_player_tmp[i]);
        //}
        dataController.dataManagement.saveData(); // Save the data to disk
        GameObject.Find("Player/UI/levelComplete/highscoreText").GetComponent<Text>().text = "Highscore: " + dataController.dataManagement.highscores_tmp[0, levelNum]; // Put the highest score ever in this level up
        Text highscore_text = GameObject.Find("Player/UI/levelComplete/highscores").GetComponent<Text>(); // Where the highscores are listed
        highscore_text.text = "Highscores\n1: " + dataController.dataManagement.highscore_player_tmp[0, levelNum] + " : " + dataController.dataManagement.highscores_tmp[0, levelNum] + "\n"; // A lot of code just to print out the top 10 scores, but it works... so there's that
        if (dataController.dataManagement.highscore_player_tmp[1, levelNum] != null)
        {
            highscore_text.text += "2: " + dataController.dataManagement.highscore_player_tmp[1, levelNum] + " : " + dataController.dataManagement.highscores_tmp[1, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[2, levelNum] != null)
        {
            highscore_text.text += "3: " + dataController.dataManagement.highscore_player_tmp[2, levelNum] + " : " + dataController.dataManagement.highscores_tmp[2, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[3, levelNum] != null)
        {
            highscore_text.text += "4: " + dataController.dataManagement.highscore_player_tmp[3, levelNum] + " : " + dataController.dataManagement.highscores_tmp[3, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[4, levelNum] != null)
        {
            highscore_text.text += "5: " + dataController.dataManagement.highscore_player_tmp[4, levelNum] + " : " + dataController.dataManagement.highscores_tmp[4, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[5, levelNum] != null)
        {
            highscore_text.text += "6: " + dataController.dataManagement.highscore_player_tmp[5, levelNum] + " : " + dataController.dataManagement.highscores_tmp[5, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[6, levelNum] != null)
        {
            highscore_text.text += "7: " + dataController.dataManagement.highscore_player_tmp[6, levelNum] + " : " + dataController.dataManagement.highscores_tmp[6, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[7, levelNum] != null)
        {
            highscore_text.text += "8: " + dataController.dataManagement.highscore_player_tmp[7, levelNum] + " : " + dataController.dataManagement.highscores_tmp[7, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[8, levelNum] != null)
        {
            highscore_text.text += "9: " + dataController.dataManagement.highscore_player_tmp[8, levelNum] + " : " + dataController.dataManagement.highscores_tmp[8, levelNum] + "\n";
        }
        if (dataController.dataManagement.highscore_player_tmp[9, levelNum] != null)
        {
            highscore_text.text += "10: " + dataController.dataManagement.highscore_player_tmp[9, levelNum] + " : " + dataController.dataManagement.highscores_tmp[9, levelNum] + "\n";
        }

    }

    void setPlayerName(string name)
    {
        playerName = name; // Set local variable to the incoming variable
    }

    void OnTriggerEnter2D(Collider2D collision) // For detecting collisions with coins
    {
        if(collision.tag == "coin") // Yep, see, collision with a coin
        {
            coins_collected++; // Count the coin
            coinTink.PlayOneShot(coinSound); // Play the sound of a coin being collected
            Destroy(collision.gameObject); // Destroy the coin
        }
    }
}
