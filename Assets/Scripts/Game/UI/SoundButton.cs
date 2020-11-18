using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public void MuteButton()
    {
        AudioManager.instance.MuteAll();
    }
    public void UnmuteButton()
    {
        AudioManager.instance.UnmuteAll();
    }
}
