using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreController : MonoBehaviour
{
    private float timer;
    private bool time_run;
    public Text timer_UI;
    public int minTime; // Minimum time to complete level (minimum time to play through level for developer divided by two or so for skill ceiling)
    public int failTime; // Time beyond which score will be base, no time bonus
    public int baseScore; // Base score for level... kind of silly, but at least if they get no time bonus they don't get 0 score
    public string playerName; // The name of the player, found at the end of level

    void Start()
    {
        dataController.dataManagement.loadData(); // Load saved data
        //print("Data: " + dataController.dataManagement.highscores_tmp[0]);
        timer = 0.0f;
        time_run = true;
        playerName = "";
    }

    void Update()
    {
        if(time_run == true)
        {
            timer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        timer_UI.text = (""+(int)timer);
    }

    void stopTimer()
    {
        time_run = false;
    }

    public int calculateScore()
    {
        stopTimer();
        int score = 0;
        int health = GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller>().health;
        if(timer < minTime) // Make sure time doesn't go negative when minTime is subtracted off their time
        {
            minTime = (int)timer;
        }
        int time_score_func = failTime - ((int)timer - minTime);
        if(time_score_func < 0) // If they are over failTime, then just give them a time bonus of 0
        {
            time_score_func = 0;
        }
        score = baseScore + health * time_score_func;
        handleHighScore(score);
        return score;
    }

    private void handleHighScore(int score)
    {
        int prevBest = 0, curBest = 0;
        string prevPlayer = "", curPlayer = "";
        bool foundPlace = false;
        for (int i = 0; i < 10; i++){
            if (foundPlace) // Runs on loop after place is found
            { // Swaps the previous higher score with the next lower score, place 10 (or 9 since starts at 0) falls off list
                curBest = dataController.dataManagement.highscores_tmp[i];
                curPlayer = dataController.dataManagement.highscore_player_tmp[i];
                dataController.dataManagement.highscores_tmp[i] = prevBest;
                dataController.dataManagement.highscore_player_tmp[i] = prevPlayer;
                prevBest = curBest;
                prevPlayer = curPlayer;
            }
            if (score >= dataController.dataManagement.highscores_tmp[i] && foundPlace == false) // Until we find the place for the current score, loop forever
            {
                foundPlace = true;
                prevBest = dataController.dataManagement.highscores_tmp[i];
                prevPlayer = dataController.dataManagement.highscore_player_tmp[i];
                dataController.dataManagement.highscores_tmp[i] = score;
                dataController.dataManagement.highscore_player_tmp[i] = playerName;
            }            
        }
        GameObject.Find("Player/UI/levelComplete/highscoreText").GetComponent<Text>().text = "Highscore: " + dataController.dataManagement.highscores_tmp[0];
    }

    void setPlayerName(string name)
    {
        playerName = name;
    }
}
