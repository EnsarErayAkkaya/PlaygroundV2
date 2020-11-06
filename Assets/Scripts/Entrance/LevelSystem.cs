using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public GameObject levelButton;
    public Transform levelsContent;
    public ZenSceneDataManager sceneDataManager;
    public EntranceUI entranceUI;

    void Start()
    {
        entranceUI = FindObjectOfType<EntranceUI>();
        sceneDataManager = GameDataManager.instance.zenSceneDataManager;
        Set();
    }

    public void Set()
    {
        foreach (Transform child in levelsContent)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in GameDataManager.instance.levels)
        {
            LevelButton lb = Instantiate(levelButton, levelsContent).GetComponent<LevelButton>();

            if(SaveAndLoadGameData.instance.savedData.unlockedLevels.Any(s => s.levelIndex == item.levelIndex))
            {
                LevelData ld = SaveAndLoadGameData.instance.savedData.unlockedLevels.First(s => s.levelIndex == item.levelIndex);
                
                lb.Set(item.levelIndex, ld.mark, ld.isUnlocked );
                
                if(ld.isUnlocked)
                    lb.GetComponent<Button>().onClick.AddListener( delegate{ LevelButtonOnClick(item); } );
            }
            else
            {
                lb.Set(item.levelIndex, 0, false );
            }
            //lb.transform.SetParent(levelsContent, false);
        }
    }

    public void LevelButtonOnClick(LevelData ld)
    {
        if(ld.levelSceneIndex != -1)
        {
            SceneManager.LoadScene(ld.levelSceneIndex);
            GameDataManager.instance.currentlyPlayingLevelIndex = ld.levelIndex - 1;
        }
        else
        {
            GameDataManager.instance.currentlyPlayingLevelIndex = ld.levelIndex - 1;
            sceneDataManager.LoadingScene = ld.levelData;
            sceneDataManager.isLoad = true;
            entranceUI.OpenLevelScene();
        }
    }
}
