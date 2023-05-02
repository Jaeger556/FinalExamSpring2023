using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Exit : MonoBehaviour
{
    public Text HighScores;
    public int num_scores = 10;


    //Loads high scores
    void Start()
    {
        string path = "Assets/scores.txt";
        string line;
        string[] fields;
        string[] playerNames = new string [num_scores];
        int[] playerScores = new int[num_scores];
        int scores_read = 0;

        HighScores.text = "";
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream && scores_read < num_scores)
        {
            line = reader.ReadLine();
            fields = line.Split(',');
            HighScores.text += fields[0] + ": " + fields[1] + "\n";
            scores_read += 1;
        }
        reader.Close();
    }

    //Closes application
    public void exit()
    {
        Debug.Log("Closing application.");
        Application.Quit();
    }

    //Loads intro scene
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    //Clears the scores.txt file and sets a default score
    public void ResetScores()
    {
        Debug.Log("Clearing score.txt");

        string path = "Assets/scores.txt";

        using (var stream = new FileStream(path, FileMode.Truncate))
        {
            using(var writer = new StreamWriter(stream))
            {
                writer.Write("default,0");
            }
        }
    }
}
