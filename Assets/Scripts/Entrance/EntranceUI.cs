using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceUI : MonoBehaviour
{
    public Transform myContent;
    void Start()
    {
        /* foreach (RailType type in SaveAndLoadGameData.instance.savedData.playerRails)
        {
            GameObject a = Instantiate(GameDataManager.instance.allRails.Find(s => s.railType == type).railButton);
            a.transform.SetParent(myContent);
        } */
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
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene(1);
    }
     public void OpenLevelScene()
    {
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene("LevelScene");
    }
}
