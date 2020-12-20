using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostProccessing : MonoBehaviour
{
    Toggle t;
    EntranceCamera entranceCamera;
    private void Start()
    {
        t = GetComponent<Toggle>();
        entranceCamera = FindObjectOfType<EntranceCamera>();
        if (SaveAndLoadGameData.instance.savedData.postProccessing)
        {
            t.isOn = true;
        }
        else
        {
            t.isOn = false;
        }
    }
    public void OnToggleChanged()
    {
        if(t.isOn) // set PP on
        {
            SaveAndLoadGameData.instance.savedData.postProccessing = true;
        }
        else
        {
            SaveAndLoadGameData.instance.savedData.postProccessing = false;
        }
        SaveAndLoadGameData.instance.Save();
        entranceCamera.UpdatePP();
    }
}
