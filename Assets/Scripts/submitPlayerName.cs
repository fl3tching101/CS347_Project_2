using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submitPlayerName : MonoBehaviour
{

    private Text nameInput;
    private string nameFromInput;
    private GameObject scoreCont;
    private GameObject inputField;

    private void Start()
    {
        nameInput = GameObject.Find("Player/UI/playerNameUI/InputField/Text").GetComponent<Text>();
        scoreCont = GameObject.Find("Player").GetComponent<scoreController>().gameObject;
        inputField = GameObject.Find("Player/UI/playerNameUI");
    }
    public void submitName()
    {
        nameFromInput = nameInput.text;
        scoreCont.SendMessage("setPlayerName", nameFromInput);
        inputField.SetActive(false);
        GameObject.Find("Player/UI/levelComplete").SetActive(true); // Display the level complete UI text 
        int score = GameObject.Find("Player").GetComponent<scoreController>().calculateScore();
        GameObject.Find("Player/UI/levelComplete/scoreText").GetComponent<Text>().text = "Your Score: " + score;
    }
}
