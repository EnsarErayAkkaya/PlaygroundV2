using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelDataManager : MonoBehaviour
{
    void Awake()
    {
        RailManager railManager = FindObjectOfType<RailManager>();
        LevelManager levelManager = FindObjectOfType<LevelManager>();
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
        levelManager.SetBudgetFirstTime(ld.budget);
        levelManager.levelRails = ld.choosenRails;

        levelManager.targetedTrainCount = ld.levelData.trainsData.Count;
    }
}
