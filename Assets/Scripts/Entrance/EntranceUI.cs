using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceUI : MonoBehaviour
{
    public Transform myContent;
    void Start()
    {
        GameDataManager.instance.zenSceneDataManager.LoadingScene = null;
        GameDataManager.instance.zenSceneDataManager.isLoad = false;

        if(SaveAndLoadGameData.instance.savedData.unlockedLevels.Count < 1)
        {
            SaveAndLoadGameData.instance.savedData.unlockedLevels.Add(new PlayerLevelData(){
                levelIndex = 1,
                mark = 0
            });
        }
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
