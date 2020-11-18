using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public void MuteButton()
    {
        AudioManager.instance.MuteTheme();
    }
    public void UnmuteButton()
    {
        AudioManager.instance.UnmuteTheme();
    }
}
