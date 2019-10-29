using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submitPlayerName : MonoBehaviour
{

    private Text nameInput;         // Where the name goes
    private string nameFromInput;   // The actual string from the item above
    private GameObject scoreCont;   // Score controller
    private GameObject inputField;  // Allows visibility of the UI to be set

    private void Start()
    {
        nameInput = GameObject.Find("Player/UI/playerNameUI/InputField/Text").GetComponent<Text>();     // The text that holds the players name
        scoreCont = GameObject.Find("Player").GetComponent<scoreController>().gameObject;               // Score controller object
        inputField = GameObject.Find("Player/UI/playerNameUI");                                         // The UI group that this menu corresponds to
    }   
    public void submitName()
    {
        nameFromInput = nameInput.text; // Get the name
        if (nameFromInput != "") // Don't let them input nothing
        {
            scoreCont.SendMessage("setPlayerName", nameFromInput); // Give the name to the score controller
            inputField.SetActive(false); // Disable the UI group now that we have the name
            GameObject.Find("Player/UI/levelComplete").SetActive(true); // Display the level complete UI text 
            int score = GameObject.Find("Player").GetComponent<scoreController>().calculateScore(); // Figure out the points
            GameObject.Find("Player/UI/levelComplete/scoreText").GetComponent<Text>().text = "Your Score: " + score; // Tell them what they got
        }
    }
}
