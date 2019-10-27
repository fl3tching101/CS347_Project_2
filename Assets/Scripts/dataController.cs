using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class dataController : MonoBehaviour
{
    public static dataController dataManagement;
    public int[] highscores_tmp = new int[10];
    public string[] highscore_player_tmp = new string[10];

    private void Awake()
    {
        if(dataManagement == null)
        {
            DontDestroyOnLoad(gameObject);
            dataManagement = this;
        }else if(dataManagement != this)
        {
            Destroy(gameObject);
        }
    }

    public void saveData()
    {
        BinaryFormatter binForm = new BinaryFormatter(); // Binary formatter object
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat"); // Creates game save file
        gameData data = new gameData(); // Container
        data.highscores = highscores_tmp;
        data.highscore_player = highscore_player_tmp;
        binForm.Serialize(file, data);
        file.Close();
    }
    public void loadData()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            BinaryFormatter binForm = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            gameData data = (gameData)binForm.Deserialize(file);
            file.Close();
            highscores_tmp = data.highscores;
            highscore_player_tmp = data.highscore_player;
        }
    }
}

[Serializable]
class gameData
{
    public int[] highscores = new int[10];
    public string[] highscore_player = new string[10];
}