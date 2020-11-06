using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager: MonoBehaviour
{
    public static GameDataManager instance;
    public int currentlyPlayingLevelIndex;
    public ZenSceneDataManager zenSceneDataManager;
    public List<LevelData> levels;
    public List<RailData> allRails;
    public List<EnvironmentData> allEnvs;
    public List<PlaygroundData> allPlaygrounds;
    public List<TrainData> allTrains;

    public CreatedLevels createdLevels;

    void Awake()
    {
        levels = createdLevels.createdLevels;
		if(instance != null)
		{
			Debug.LogWarning("More than one instance of DataManager found");
			return;
		}
		instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void AddNewPlayerRail(RailType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerRails.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerRails.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void AddNewPlayerEnvironment(EnvType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerEnvs.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerEnvs.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void AddNewPlayerPlayground(PlaygroundType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerPlaygrounds.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerPlaygrounds.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }

    public void ChoosePlayground(PlaygroundType playgroundType)
    {
        if(SaveAndLoadGameData.instance.savedData.choosenPlayground != playgroundType )
        {
            SaveAndLoadGameData.instance.savedData.choosenPlayground = playgroundType;
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void AddNewPlayerTrain(TrainType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerTrains.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerTrains.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void SaveLevelMark(int _mark)
    {
        SaveAndLoadGameData.instance.savedData.unlockedLevels.First( s => s.levelIndex == currentlyPlayingLevelIndex).mark = _mark;
                
        SaveAndLoadGameData.instance.Save();
    }
    public void UnlockNextLevel()
    {
        if(levels.Count > currentlyPlayingLevelIndex )
        {
            if( SaveAndLoadGameData.instance.savedData.unlockedLevels.Any(s => s.levelIndex == currentlyPlayingLevelIndex+1) == false )
            {
                LevelData ld = levels[currentlyPlayingLevelIndex];
                ld.isUnlocked = true;
                SaveAndLoadGameData.instance.savedData.unlockedLevels.Add(ld);
                    
                SaveAndLoadGameData.instance.Save();
            }
        }
    }
    public void SaveCreatedLevel(ZenSceneData levelSceneData, int Budget, List<RailType> ChoosenRails)
    {
        Debug.Log("level data saving... ");
        LevelData levelData;
        
        if(createdLevels.createdLevels.Count > 0)
        {
            LevelData lastLevel = createdLevels.createdLevels.Last();
            levelData = new LevelData()
            {
                levelIndex = lastLevel.levelIndex + 1,
                levelSceneIndex = -1,
                isUnlocked = false,
                mark = 0,
                choosenRails = ChoosenRails,
                budget = Budget,
                levelData = levelSceneData
            };
        }
        else
        {
            levelData = new LevelData()
            {
                levelIndex = 1,
                levelSceneIndex = -1,
                isUnlocked = true,
                mark = 0,
                choosenRails = ChoosenRails,
                budget = Budget,
                levelData = levelSceneData
            };
        }       
        createdLevels.createdLevels.Add(levelData);
        levels = createdLevels.createdLevels;
    }
}
