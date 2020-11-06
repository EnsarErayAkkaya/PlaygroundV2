using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class GameControllUI : MonoBehaviour
{
    public GameUIManager uiManager;
    public GameObject restart, quit, levelCreatingModeToggle;
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
            if(!isLevel)
                levelCreatingModeToggle.SetActive(false);
        }
        else
        {
            isShowing = true;
            restart.SetActive(true);
            quit.SetActive(true);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
