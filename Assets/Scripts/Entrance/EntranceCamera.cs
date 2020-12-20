using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EntranceCamera : MonoBehaviour
{
    public Volume volume;

    private void Start()
    {
        UpdatePP();
    }
    public void UpdatePP()
    {
        if (SaveAndLoadGameData.instance.savedData.postProccessing)
        {
            volume.enabled = true;
        }
        else
        {
            volume.enabled = false;
        }
    }
}
