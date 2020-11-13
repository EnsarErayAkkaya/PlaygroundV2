using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]Transform content;   
    void Start()
    {
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerRails)
        {
            RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);

            GameObject e = Instantiate(GameDataManager.instance.generalButtonPrefab, content);
            e.transform.GetChild(0).GetComponent<Image>().sprite = data.railImage;
            e.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.name;
        }
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerEnvs)
        {
            EnvironmentData data = GameDataManager.instance.allEnvs.Find(s => s.envType == item);
            GameObject e = Instantiate(GameDataManager.instance.generalButtonPrefab, content);
            e.transform.GetChild(0).GetComponent<Image>().sprite = data.envImage;
            e.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.name;
        }
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerTrains)
        {
            TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);
            GameObject e = Instantiate(GameDataManager.instance.generalButtonPrefab, content);
            e.transform.GetChild(0).GetComponent<Image>().sprite = data.trainImage;
            e.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.name;
        }
    }
}
