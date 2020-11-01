using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]Transform content;   
    void Start()
    {
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerRails)
        {
            RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);
            GameObject e = Instantiate(data.railButton);
            e.transform.SetParent(content, false);
        }
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerEnvs)
        {
            EnvironmentData data = GameDataManager.instance.allEnvs.Find(s => s.envType == item);
            GameObject e = Instantiate(data.envButton);
            e.transform.SetParent(content, false);
        }
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerTrains)
        {
            TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);
            GameObject e = Instantiate(data.trainButton);
            e.transform.SetParent(content, false);
        }
    }
}
