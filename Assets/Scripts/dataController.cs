using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class dataController : MonoBehaviour
{
    public static dataController dataManagement;
    public int[,] highscores_tmp;
    public string[,] highscore_player_tmp;
    

    void Awake()
    {
        if (dataManagement == null)
        {
            DontDestroyOnLoad(gameObject);
            dataManagement = this;
        }
        else if (dataManagement != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        highscores_tmp = new int[10,3];
        highscore_player_tmp = new string[10,3];
        //print(Application.persistentDataPath);
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
            //for(int i = 0; i < 10; i++)
            //{
            //    print("Score for place " + i + " : " + highscores_tmp[i]);
            //    print("Player for place " + i + " : " + highscore_player_tmp[i]);
            //}
        }
    }
}

[Serializable]
class gameData
{
    public int[,] highscores;
    public string[,] highscore_player;
}