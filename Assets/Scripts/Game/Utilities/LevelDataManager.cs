using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelDataManager : MonoBehaviour
{
    LevelManager levelManager;
    void Awake()
    {
        RailManager railManager = FindObjectOfType<RailManager>();
        levelManager = FindObjectOfType<LevelManager>();
        GameDataManager.instance.zenSceneDataManager.LoadZenSceneData();
        foreach (Rail item in railManager.GetRails())
        {
            railManager.ConnectCollidingRailPoints(item);
            if( item.isEnd )
            {
                levelManager.targetRail = item; 
            }
        }
        LevelData ld = GameDataManager.instance.levels[GameDataManager.instance.currentlyPlayingLevelIndex];
        
        SetLevelContent( ld.levelContent );

        levelManager.targetedTrainCount = ld.levelData.trainsData.Count;
    }
    void SetLevelContent(LevelContentData levelContent)
    {
        levelManager.SetBudgetFirstTime(levelContent.budget);

        levelManager.levelRails = levelContent.levelRails;
        levelManager.levelEnvs = levelContent.levelEnvs;
        levelManager.levelTrains = levelContent.levelTrains;

        levelManager.levelRailPrize = levelContent.rewardRails.ToArray();
        levelManager.levelEnvPrize = levelContent.rewardEnvs.ToArray();
        levelManager.levelTrainPrize = levelContent.rewardTrains.ToArray();
    }
}
