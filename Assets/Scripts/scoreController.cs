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

    void Start()
    {
        timer = 0.0f;
        time_run = true;
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
        return score;
    }
}
