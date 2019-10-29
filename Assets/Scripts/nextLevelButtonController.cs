using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevelButtonController : MonoBehaviour
{
    public string nextScene; // Which scene to go to

    public void moveNextLevel()
    {
        SceneManager.LoadScene(nextScene); // Go to that scene
    }
}
