using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject deleteButton;
    public GameObject levelNameObject;
    public GameObject editButton;

    private int index;
    public TextMeshProUGUI levelIndexText, levelName;
    public Image starImage;
    public Image background;
    public Sprite[] stars;
    public Color locked;
    public Color unLocked;
    
    LevelSystem levelSystem; 

    public void Set(int lvlIndex, int mark, bool isUnlocked, LevelSystem ls, string name )
    {
        levelSystem = ls;

        index = lvlIndex - 1;
        levelName.text = name;
        levelNameObject.gameObject.SetActive(false);

        levelIndexText.text = lvlIndex.ToString();

        if(mark > 0)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = stars[mark-1];
        }
        if(isUnlocked)
        {
            background.color = unLocked;
        }
        else
            background.color = locked;
    }

    public void DeleteLevelButton()
    {
        if(GameDataManager.instance.levels[index].isMaster == false )
        {
            levelSystem.DeleteLevel(index, this);
        }
    }
    public void LeftButton()
    {
        if(GameDataManager.instance.levels[index].isMaster == false )
        {
            levelSystem.MoveLevelLeft(index);
        }
    }
    public void RightButton()
    {
        if(GameDataManager.instance.levels[index].isMaster == false )
        {
            levelSystem.MoveLevelRight(index);
        }
    }
    public void EditButton()
    {
        if (GameDataManager.instance.levels[index].isMaster == false)
        {
            levelSystem.EditLevel(index);
        }
    }
    public bool ShowEditButtons()
    {
        if(GameDataManager.instance.levels[index].isMaster == false )
        {
            deleteButton.SetActive(true);
            leftButton.SetActive(true);
            rightButton.SetActive(true);
            editButton.SetActive(true);
            levelNameObject.gameObject.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void HideEditButtons()
    {
        if(GameDataManager.instance.levels[index].isMaster == false )
        {
            deleteButton.SetActive(false);
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            editButton.SetActive(false);
            levelNameObject.gameObject.SetActive(false);
        }
    }
}
