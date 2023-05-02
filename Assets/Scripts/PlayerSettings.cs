using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private AudioSource music;

    //Checks prefs for music save and loads the music toggle state
    public void Awake()
    {
        if(!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 1);
            toggle.isOn = true;
            music.enabled = true;
            PlayerPrefs.Save();
        }
        else
        {
            if(PlayerPrefs.GetInt("music") == 0)
            {
                music.enabled = false;
                toggle.isOn = false;
            }
            else
            {
                music.enabled = true;
                toggle.isOn = true;
            }
        }
    }

    //Toggles music and saves the buttons state
    public void ToggleMusic()
    {
        if(toggle.isOn)
        {
            PlayerPrefs.SetInt("music", 1);
            music.enabled = true;
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
            music.enabled = false;
        }
        PlayerPrefs.Save();
    }
}
