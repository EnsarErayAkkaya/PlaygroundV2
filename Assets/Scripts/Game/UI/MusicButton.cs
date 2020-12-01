using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public GameObject on;
    public GameObject off;
    private void Start() {
        if(!SaveAndLoadGameData.instance.savedData.musicON) // müzik kapalı ise
        {
            AudioManager.instance.MuteTheme(); // müziği kapat
            WhenMusicOff();
        }
        else
        {
            AudioManager.instance.UnmuteTheme();
            WhenMusicOn();
        }
    }
    public void MuteButton()
    {
        AudioManager.instance.MuteTheme();
        WhenMusicOff();
        GameDataManager.instance.SetMusic(false);
    }
    public void UnmuteButton()
    {
        AudioManager.instance.UnmuteTheme();
        WhenMusicOn();
        GameDataManager.instance.SetMusic(true);
    }
    void WhenMusicOn()
    {
        off.SetActive(false);
        on.SetActive(true);
    } void WhenMusicOff()
    {
        off.SetActive(true);
        on.SetActive(false);
    }
}
