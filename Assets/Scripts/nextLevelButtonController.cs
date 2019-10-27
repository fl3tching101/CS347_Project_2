using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevelButtonController : MonoBehaviour
{
    public string nextScene;

    public void moveNextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
