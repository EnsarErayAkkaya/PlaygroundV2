using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    public RectTransform storyPanel;
    public TextWriterUI textWriterUI;
    public List<StoryHelper> stories;
    bool nextButtonPressed;

    private void Start()
    {
        if (FindObjectOfType<LevelUI>() == null)
        {
            gameObject.SetActive(false);
            enabled = false;
        }
        else
        {
            if (GameDataManager.instance.currentlyPlayingLevelIndex == 0)
            {
                ShowRelatedStory(0);
            }
            else if (GameDataManager.instance.currentlyPlayingLevelIndex == 1)
            {
                ShowRelatedStory(1);
            }
            else if (GameDataManager.instance.currentlyPlayingLevelIndex == 2)
            {
                ShowRelatedStory(2);
            }
            else if (GameDataManager.instance.currentlyPlayingLevelIndex == 3)
            {
                ShowRelatedStory(4);
            }
            else if (GameDataManager.instance.currentlyPlayingLevelIndex == 4)
            {
                ShowRelatedStory(3);
            }
        }

    }
    public void ShowRelatedStory(int storyIndex)
    {
        ShowPanel();
        StartCoroutine(StoryRoutine(storyIndex));
    }
    IEnumerator StoryRoutine(int storyIndex)
    {
        int textIndex = 0;

        while (textIndex < stories[storyIndex].texts.Count)
        {
            TextHelper text = stories[storyIndex].texts[textIndex];

            if (text.isAndroid) // on Android
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    textIndex += 1;
                    continue;
                }
            }
            else if (text.isPC) // on PC
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    textIndex += 1;
                    continue;
                }
            }
                
            storyPanel.anchoredPosition += text.textPanelPos;

            textWriterUI.story = text.text;
            textWriterUI.CallPlayText();

            textIndex += 1;

            while (!Input.GetKeyDown(KeyCode.Return) && !nextButtonPressed)
            {
                yield return null;
            }

            nextButtonPressed = false;

            if (textWriterUI.writing)
            {
                textWriterUI.cancelWriting = true;
            }

            yield return null;
        }

        CloseStory();
    }
    

    void CloseStory()
    {
        HidePanel();
    }
    public void ShowPanel()
    {
        storyPanel.gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        storyPanel.gameObject.SetActive(false);
    }
    public void NextButtonClick()
    {
        nextButtonPressed = true;
    }
}
[System.Serializable]
public class TextHelper
{
    public string text;
    public bool isAndroid = false;
    public bool isPC = false;
    public Vector2 textPanelPos;
}
[System.Serializable]
public class StoryHelper
{
    public string Name;
    public List<TextHelper> texts;
    public Vector2 textPanelDefaultPos;
}