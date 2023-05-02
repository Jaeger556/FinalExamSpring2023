using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public InputField playerName;
    public Dropdown lives;
    public Slider timeSlider;
    public Text sliderVal;

    //Updates sliders text value
    void Update()
    {
        sliderVal.text = timeSlider.value + " Seconds";
    }

    //Loads game scene and sets values for name and timer
    public void play()
    {
        Game.playernamestr = playerName.text;
        Game.playertimeleft = timeSlider.value;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //List for live selection
    public void SelectLives()
    {
        switch(lives.value)
        {
            case 1:
                Game.playerlivesint = 1;
                break;
            case 2:
                Game.playerlivesint = 2;
                break;
            case 3:
                Game.playerlivesint = 3;
                break;
            case 4:
                Game.playerlivesint = 4;
                break;
            case 5:
                Game.playerlivesint = 5;
                break;
            case 6:
                Game.playerlivesint = 6;
                break;
            case 7:
                Game.playerlivesint = 7;
                break;
            case 8:
                Game.playerlivesint = 8;
                break;
            case 9:
                Game.playerlivesint = 9;
                break;
        }
    }
}
