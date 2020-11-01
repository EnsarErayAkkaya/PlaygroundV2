﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class LevelUI : MonoBehaviour
{
    public LevelManager levelManager;
    [SerializeField] GameObject endUI;
    [SerializeField] Image markImage;
    [SerializeField] Button nextButton,restartButton;
    [SerializeField] TextMeshProUGUI lostText, rewardText, budgetText;
    public Sprite[] stars;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }
    public bool SetBudget(int c)
    {
        bool a = levelManager.SetBudget(c);
        if(a)
        {
            SetBudgetText();
            return true;
        }
        else
        {
            Debug.Log("Not enough resources");
            return false;
        }
    }
    void SetBudgetText()
    {
        budgetText.text = levelManager.budget.ToString();
    }
    public void SetEndUI(int m)
    {
        endUI.SetActive(true);

        if(m == 0 ) // you lost
        {
            markImage.gameObject.SetActive(false);
            lostText.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        else // you passed
        {
            markImage.sprite = stars[m-1];
            rewardText.gameObject.SetActive(true);
            StartCoroutine( FadeTextToZeroAlpha() );
        
            if( GameDataManager.instance.levels.Any(s => s.levelIndex == GameDataManager.instance.currentlyPlayingLevelIndex+1) )
                nextButton.gameObject.SetActive(true);
            else
                nextButton.gameObject.SetActive(false);
        }
    }
    public void NextLevelButtonClick()
    {
        LevelData ld = GameDataManager.instance.levels.First( s => s.levelIndex == GameDataManager.instance.currentlyPlayingLevelIndex + 1 );
        GameDataManager.instance.currentlyPlayingLevelIndex = ld.levelIndex;
        SceneManager.LoadScene(ld.levelSceneIndex);
    }
    public void RestartLevelButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator FadeTextToZeroAlpha()
    {
        rewardText.color = new Color(rewardText.color.r, rewardText.color.g, rewardText.color.b, 1);
        while (rewardText.color.a > 0.0f)
        {
            rewardText.color = new Color(rewardText.color.r, rewardText.color.g, rewardText.color.b, rewardText.color.a - (Time.deltaTime / 5));
            yield return null;
        }
         rewardText.gameObject.SetActive(false);
    }
}
