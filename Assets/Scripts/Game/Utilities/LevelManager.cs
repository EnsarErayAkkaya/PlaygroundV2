using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level geçildiğinide ödülleri verecek oyun sonu ekranını tetikleyecek
    RailManager railManager;
    LevelUI levelUI;
    CollectableUIManager collectableUIManager;

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
        collectableUIManager = FindObjectOfType<CollectableUIManager>();
        
        gdm = GameDataManager.instance;
        railManager.AddCreatedRails();
    }

    
    public bool TrainReachedTarget(Rail r)
    {
        if(r == targetRail)
        {
            reachedTrainCount++;
            if( reachedTrainCount == targetedTrainCount )
            {
                EndLevel();
            }
            return true;
        }
        return false;
    }
    public void TrainCollided()
    {
        levelUI.SetEndUI(0);
    }
    public void Reset()
    {
        reachedTrainCount = 0;
        collectedCount = 0;
        isGameEnded = false;
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
        int x = 0;
        if(collectableCount > 0)
        {
            float multiplier = 60 / collectableCount;
            float successCount = collectedCount * multiplier;
            float successPrecent = (60.0f / successCount) * 100.0f;
            if( successPrecent > 66 &&  successPrecent <= 100 )
            {
                x = 3;
            }
            else if( successPrecent > 33 &&  successPrecent <= 66 )
            {
                x = 2;
            }
            else if( successPrecent >= 0 &&  successPrecent <= 33 )
            {
                x = 1;
            }
            else
            {
                x = 1;
            }
        
        }
        else
        {
            x = 3;
        }
        
        levelUI.SetEndUI(x);
        return x;
    }
    public void EndLevel()
    {
        AudioManager.instance.Play("Celebration");
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
        
        collectableUIManager.OnCollected(collectedCount);

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
