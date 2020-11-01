using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameControllUI : MonoBehaviour
{
    public GameObject restart, quit;
    bool isShowing;
    public void TogglePanel()
    {
        if(isShowing)
        {
            isShowing = false;
            restart.SetActive(false);
            quit.SetActive(false);
        }
        else
        {
            isShowing = true;
            restart.SetActive(true);
            quit.SetActive(true);
        }
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
