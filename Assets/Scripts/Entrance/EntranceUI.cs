using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceUI : MonoBehaviour
{
    public TextMeshProUGUI leaf;
    void Start()
    {
        GameDataManager.instance.zenSceneDataManager.LoadingScene = null;
        GameDataManager.instance.zenSceneDataManager.isLoad = false;
        GameDataManager.instance.zenSceneDataManager.isLevel = false;
        GameDataManager.instance.zenSceneDataManager.levelIndex = -1;   

        if (!SaveAndLoadGameData.instance.savedData.soundON)
            AudioManager.instance.MuteAll();
        else
            AudioManager.instance.UnmuteAll();

        if (!SaveAndLoadGameData.instance.savedData.musicON) // müzik kapalı ise
            AudioManager.instance.MuteTheme(); // müziği kapat
        else
            AudioManager.instance.UnmuteTheme();

        if (SaveAndLoadGameData.instance.savedData.unlockedLevels.Count < 1)
        {
            SaveAndLoadGameData.instance.savedData.unlockedLevels.Add(new PlayerLevelData(){
                levelIndex = 1,
                mark = 0
            });
        }

        UpdateLeaftext();
    }

    public void UpdateLeaftext()
    {
        leaf.text = SaveAndLoadGameData.instance.savedData.leaf.ToString();
    }
    
    public void OpenZenScene()
    {
        AudioManager.instance.Stop("TrainMoving");
        AudioManager.instance.Stop("TrainHorn");
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene(1);
    }
    public void OpenLevelScene()
    {
        AudioManager.instance.Stop("TrainMoving");
        AudioManager.instance.Stop("TrainHorn");
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene("LevelScene");
    }
}
