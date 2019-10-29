using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class dataController : MonoBehaviour
{
    public static dataController dataManagement; 
    public int[,] highscores_tmp; // 10 x 3 array for the 3 levels highscores
    public string[,] highscore_player_tmp; // 10 x 3 array for the levels highscore earners
    

    void Awake() // Singleton class, means that the data won't get lost from what I understand
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
        highscores_tmp = new int[10,3]; // Initiate the size of the array
        highscore_player_tmp = new string[10,3]; // Initate the size of the array
        //print(Application.persistentDataPath); // Used this to figure out where the highscore table is
    }

    public void saveData() // Save data to disk
    {
        BinaryFormatter binForm = new BinaryFormatter();                                        // Binary formatter object
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");        // Creates game save file
        gameData data = new gameData();                                                         // Container
        data.highscores = highscores_tmp;                                                       // Set the stored data to equal the temporary data
        data.highscore_player = highscore_player_tmp;                                           // Set the stored data to equal the temporary data
        binForm.Serialize(file, data);                                                          // Serialize the data for storage
        file.Close();                                                                           // Close the file
    }
    public void loadData() // Load data from disk
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat")) // Grab the saved highscore data, if it exists
        {
            BinaryFormatter binForm = new BinaryFormatter(); // Create a binary formatter
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open); // Open the file
            gameData data = (gameData)binForm.Deserialize(file); // Decode the data, sort of
            file.Close(); // Close the file
            highscores_tmp = data.highscores; // Set local variable to stored data
            highscore_player_tmp = data.highscore_player; // Set local variable to the stored data
            //for(int i = 0; i < 10; i++)
            //{
            //    print("Score for place " + i + " : " + highscores_tmp[i]);
            //    print("Player for place " + i + " : " + highscore_player_tmp[i]);
            //}
        }
    }
}

[Serializable]
class gameData // Serializable class, makes things easier when there's more than one piece of data being stored. Also can't store multidimensional arrays without being inside a class from my understanding
{
    public int[,] highscores;
    public string[,] highscore_player;
}