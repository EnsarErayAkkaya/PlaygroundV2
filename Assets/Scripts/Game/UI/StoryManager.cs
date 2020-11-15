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

    private void Start() {
        if(FindObjectOfType<LevelUI>() == null)
        {
            gameObject.SetActive(false);    
            enabled = false;
        }
        else
        {
            if(GameDataManager.instance.currentlyPlayingLevelIndex == 0)
            {
                ShowRelatedStory(0);
            }
            else if(GameDataManager.instance.currentlyPlayingLevelIndex == 1)
            {
                ShowRelatedStory(1);
            }
            else if(GameDataManager.instance.currentlyPlayingLevelIndex == 2)
            {
                ShowRelatedStory(2);
            }
        }
        
    }
    public void ShowRelatedStory(int storyIndex)
    {
        ShowPanel();
        StartCoroutine( StoryRoutine(storyIndex) );
    }
    IEnumerator StoryRoutine(int storyIndex)
    {
        int textIndex = 0;

        while( textIndex < stories[storyIndex].texts.Count  )
        {             
            
            textWriterUI.story = stories[storyIndex].texts[textIndex].text;
            textWriterUI.CallPlayText();
            
            textIndex += 1;

            while ( !Input.GetKeyDown(KeyCode.Return) && !nextButtonPressed)
            {
                yield return null;
            }

            nextButtonPressed = false;

            if( textWriterUI.writing )
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
}
[System.Serializable]
public class StoryHelper
{
    public string Name;
    public List<TextHelper> texts;
}