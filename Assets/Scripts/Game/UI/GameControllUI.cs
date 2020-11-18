using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class GameControllUI : MonoBehaviour
{
    public GameUIManager uiManager;
    public GameObject restart, quit, levelCreatingModeToggle, soundButton, musicButton;
    bool isShowing, isLevel = false;
    public void SetIsLevel(bool isLevel = true)
    {
        this.isLevel = isLevel;
    }
    public void TogglePanel()
    {
        if(isShowing)
        {
            isShowing = false;
            restart.SetActive(false);
            quit.SetActive(false);
            soundButton.SetActive(false);
            musicButton.SetActive(false);
            if(!isLevel)
                levelCreatingModeToggle.SetActive(false);
        }
        else
        {
            isShowing = true;
            restart.SetActive(true);
            quit.SetActive(true);
            soundButton.SetActive(true);
            musicButton.SetActive(true);
            if(!isLevel)
                levelCreatingModeToggle.SetActive(true);
        }
    }
    public void ToogleLevelCreatingMode()
    {
        uiManager.ToggleLevelCreationUI();
    }
    public void RestartButtonClick()
    {
        if(uiManager.levelUI == null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            uiManager.levelUI.RestartLevelButtonClick();
        }
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
