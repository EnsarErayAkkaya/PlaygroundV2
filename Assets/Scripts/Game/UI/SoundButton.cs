using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public GameObject on;
    public GameObject off;
    private void Start() {
        if(!SaveAndLoadGameData.instance.savedData.soundON)
        {
            AudioManager.instance.MuteAll();
            off.SetActive(false);
            on.SetActive(true);
        }
        else
        {
            AudioManager.instance.UnmuteAll();
            off.SetActive(true);
            on.SetActive(false);
        }
    }
    public void MuteButton()
    {
        AudioManager.instance.MuteAll();
        off.SetActive(false);
        on.SetActive(true);
        GameDataManager.instance.SetSound(false);
    }
    public void UnmuteButton()
    {
        AudioManager.instance.UnmuteAll();
        off.SetActive(true);
        on.SetActive(false);
        GameDataManager.instance.SetSound(true);
    }
}
