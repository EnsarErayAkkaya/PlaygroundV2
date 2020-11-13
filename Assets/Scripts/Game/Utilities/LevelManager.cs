using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level geçildiğinide ödülleri verecek oyun sonu ekranını tetikleyecek
    RailManager railManager;
    LevelUI levelUI;
    
    [Header("Level Budget")]
    public int budget;
    int startingBudget;

    [Header("Level Content")]
    public List<RailType> levelRails;
    public List<EnvType> levelEnvs;
    public List<TrainType> levelTrains;

    public RailType[] levelRailPrize;
    public EnvType[] levelEnvPrize;
    public TrainType[] levelTrainPrize;

    [Header("if There is Target Rail")]
    public Rail targetRail;
    public int targetedTrainCount;
    int reachedTrainCount = 0;

    [Header("If There is Collectables")]
    public int collectableCount;
    int collectedCount;
    GameDataManager gdm;
    bool isGameEnded = false;

    void Start()
    {
        if( levelUI == null )
            levelUI = FindObjectOfType<LevelUI>();

        railManager = FindObjectOfType<RailManager>();
        
        gdm = GameDataManager.instance;
        railManager.AddCreatedRails();
        
        StartCoroutine( CheckGameEnded() );
    }

    
    public bool TrainReachedTarget(Rail r)
    {
        if(r == targetRail)
        {
            reachedTrainCount++;
            if(reachedTrainCount == targetedTrainCount)
            {
                EndLevel();
                SaveLevelData();
            }
            return true;
        }
        return false;
    }
    public void TrainCollided()
    {
        levelUI.SetEndUI(0);
    }

    IEnumerator CheckGameEnded()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);   
        while( !isGameEnded )
        {
            if( collectedCount == collectableCount && targetRail == null )
            {
                EndLevel();
            }
            else if( targetRail != null && collectedCount == collectableCount && reachedTrainCount == targetedTrainCount )
            {
                EndLevel();
            }
            yield return wait;
        }
    }

    void GivePrizes()
    {
        foreach (RailType item in levelRailPrize)
        {
            gdm.AddNewPlayerRail(item);
        }
        foreach (EnvType item in levelEnvPrize)
        {
            gdm.AddNewPlayerEnvironment(item);
        }
        foreach (TrainType item in levelTrainPrize)
        {
            gdm.AddNewPlayerTrain(item);
        }
    }
    public void SaveLevelData()
    {
        //currentLevel
        gdm.SaveLevelMark(CalculateMark());

        //unlock next Level
        gdm.UnlockNextLevel();
    }
    public int CalculateMark()
    {
        int x = 1;
        if(budget >= 0)
        {
            x = 3;
        }
        else
        {
            x = 1;
        }
        levelUI.SetEndUI(x);
        return x;
    }
    public void EndLevel()
    {
        isGameEnded = true;
        GivePrizes();
        SaveLevelData();
    }
    public bool SetBudget(int c)
    {
        int temp = budget;
        temp += c;
        if(temp < 0)
        {
            return false;
        }
        else
        {
            budget = temp;
            if(budget > startingBudget)
                budget = startingBudget;
            return true;
        }
    }
    public void TrainCollect()
    {
        collectedCount++;
        if(collectedCount > collectableCount)
            collectedCount = collectableCount;
    }
    public void SetBudgetFirstTime(int b)
    {
        budget = b;
        startingBudget = budget;
        levelUI = FindObjectOfType<LevelUI>();
        levelUI.levelManager = this;
        levelUI.SetBudgetText();
    }
}
