using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSystem : MonoBehaviour
{
    public GameObject levelButton; // prefab
    public Transform levelsContent; // buttons parent
    public List<LevelButton> levelButtons = new List<LevelButton>(); // created levels

    public GameObject editButton, stopEditButton, noEditableText;

    ZenSceneDataManager sceneDataManager;
    EntranceUI entranceUI;

    void Start()
    {
        entranceUI = FindObjectOfType<EntranceUI>();
        sceneDataManager = GameDataManager.instance.zenSceneDataManager;
        Set();
    }

    public void Set()
    {
        HideLevelButtonsEdit();
        levelButtons.Clear();
        foreach (Transform child in levelsContent)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in GameDataManager.instance.levels)
        {
            LevelButton lb = Instantiate(levelButton, levelsContent).GetComponent<LevelButton>();

            if(SaveAndLoadGameData.instance.savedData.unlockedLevels.Any(s => s.levelIndex == item.levelIndex))
            {
                LevelData ld = item;

                PlayerLevelData pld = SaveAndLoadGameData.instance.savedData.unlockedLevels.First(s => s.levelIndex == item.levelIndex);
                
                lb.Set(item.levelIndex, pld.mark, true, this );
                
                lb.GetComponent<Button>().onClick.AddListener( delegate{ LevelButtonOnClick(item); } );
            }
            else
            {
                lb.Set(item.levelIndex, 0, false, this );
            }
            levelButtons.Add( lb );
        }
    }
    public void DeleteLevel(int levelListIndex, LevelButton lb)
    {
        Debug.Log(levelListIndex);
        GameDataManager.instance.DeleteLevel(levelListIndex);
        levelButtons.Remove(lb);
        Set();
    }
    public void MoveLevelLeft(int levelListIndex)
    {
        GameDataManager.instance.MoveLevelToLeft(levelListIndex);
        Set();
    }
    public void MoveLevelRight(int levelListIndex)
    {
        GameDataManager.instance.MoveLevelToRight(levelListIndex);
        Set();
    }
    public void ShowLevelButtonsEdit()
    {
        bool isAnyEditableButton = false;
        foreach (var item in levelButtons)
        {
            isAnyEditableButton = item.ShowEditButtons();
        }

        if( !isAnyEditableButton )
        {
            editButton.SetActive(true);
            stopEditButton.SetActive(false);
            StartCoroutine( ShowNoEditableText() );
        }
        else
        {
            editButton.SetActive(false);
            stopEditButton.SetActive(true);
        }
    }
    public void HideLevelButtonsEdit()
    {
        /* Debug.Log("levelButtons " + levelButtons.Count);
        foreach (var item in levelButtons)
        {
            item.HideEditButtons();
        } */
        editButton.SetActive(true);
        stopEditButton.SetActive(false);
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
    IEnumerator ShowNoEditableText()
    {
        noEditableText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        noEditableText.SetActive(false);
    }
}
