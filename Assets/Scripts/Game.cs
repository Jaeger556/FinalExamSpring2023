using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class Game : MonoBehaviour
{
    public Text playerName;
    public Text playerLives;
    public Text playerPoints;
    public Text Timer;
    public float TimeLeft;
    public bool TimerOn = false;
    public int num_scores = 10;

    public static string playernamestr;
    public static int playerlivesint;
    public static float playertimeleft;

    private int points = 0;
    private int lives = playerlivesint;

    //Sets game UI elements when scene loads
    void Start()
    {
        TimerOn = true;
        TimeLeft = playertimeleft;
        Timer.text = playertimeleft.ToString();

        playerName.text ="Currently playing: " + playernamestr;

        playerLives.text = playerlivesint.ToString();
    }

    //Checks current time and loads exit scene if time hits zero
    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updatetimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is up!");
                TimeLeft = 0;
                TimerOn = false;
                AddNewScore();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    //Updates timer UI
    void updatetimer(float currentTime)
    {
        currentTime += 1;

        float seconds = Mathf.FloorToInt(currentTime % 60);

        Timer.text = string.Format("{00}", seconds);
    }

    //Loads the exit scene
    public void quit()
    {
        AddNewScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Increases points
    public void IncreasePoints()
    {
        points++;
        playerPoints.text = points.ToString();
    }

    //Decreases points and prevents them from dropping below zero
    public void DecreasePoints()
    {
        if(points >= 1)
        {
            points--;
            playerPoints.text = points.ToString();
        }
        else
        {
            Debug.Log("Points at zero.");
        }
    }

    //Increases lives;
    public void IncreaseLives()
    {
        lives++;
        playerLives.text = lives.ToString();
    }

    //Decreases lives and prevents them from dropping below zero
    public void DecreaseLives()
    {
        if(lives >= 1)
        {
            lives--;
            playerLives.text = lives.ToString();
        }
        else
        {
            Debug.Log("Lives at zero.");
        }
    }

    //Saves game data
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        
        save.points = points;
        save.lives = lives;
        save.time = TimeLeft;
        save.name = playernamestr;

        return save;
    }

    //Saves game data into file
    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved to " + Application.persistentDataPath);
    }

    //Loads game data into game scene
    public void LoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            playerPoints.text = save.points.ToString();
            playerLives.text = save.lives.ToString();
            points = save.points;
            lives = save.lives;
            TimeLeft = save.time;
            playerName.text = "Currently playing: " + save.name;

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved.");
        }
    }

    //Loads intro scene
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    //Saves game data into JSON file
    public void SaveAsJSON()
    {
        Save save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);

        Debug.Log("Saving as JSON: " + json);
    }

    //Adds points and name to high score file
    public void AddNewScore()
    {
        string path = "Assets/scores.txt";
        string line;
        string[] fields;
        int scores_written = 0;
        string newName = "default";
        string newScore = "999";
        bool newScoreWritten = false;
        string[] writeNames = new string[10];
        string[] writeScores = new string[10];

        newName = playernamestr;
        newScore = points.ToString();

        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream )
        {
            line = reader.ReadLine();
            fields = line.Split(',');
            if (!newScoreWritten && scores_written < num_scores) // if new score has not been written yet
            {
                //check if we need to write new higher score first
                if(Convert.ToInt32(newScore) > Convert.ToInt32( fields[1]))
                {
                    writeNames[scores_written] = newName;
                    writeScores[scores_written] = newScore;
                    newScoreWritten = true;
                    scores_written += 1;
                }

            }
            if(scores_written < num_scores) // we have not written enough lines yet
            {
                writeNames[scores_written] = fields[0];
                writeScores[scores_written] = fields[1];
                scores_written += 1;
            }
        }
        reader.Close();

        // now we have parallel arrays with names and scores to write
        StreamWriter writer = new StreamWriter(path);

        for(int x = 0; x < scores_written; x++)
        {
            writer.WriteLine(writeNames[x] + ',' + writeScores[x]);
        }
        writer.Close();

        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("scores");

    }
}
