using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public GameObject on;
    public GameObject off;
    private void Start() {
        if(!SaveAndLoadGameData.instance.savedData.musicON)
        {
            AudioManager.instance.MuteTheme();
            off.SetActive(false);
            on.SetActive(true);
        }
        else
        {
            AudioManager.instance.UnmuteTheme();
            off.SetActive(true);
            on.SetActive(false);
        }
    }
    public void MuteButton()
    {
        AudioManager.instance.MuteTheme();
        off.SetActive(false);
        on.SetActive(true);
        GameDataManager.instance.SetMusic(false);
    }
    public void UnmuteButton()
    {
        AudioManager.instance.UnmuteTheme();
        off.SetActive(true);
        on.SetActive(false);
        GameDataManager.instance.SetMusic(true);
    }
}
