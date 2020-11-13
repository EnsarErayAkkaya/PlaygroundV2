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
    public GameObject collectablePrefab;
    public GameObject generalButtonPrefab;

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
        SaveAndLoadGameData.instance.savedData.unlockedLevels.First( s => s.levelIndex == currentlyPlayingLevelIndex + 1 ).mark = _mark;
                
        SaveAndLoadGameData.instance.Save();
    }
    public void UnlockNextLevel()
    {
        if(levels.Count > currentlyPlayingLevelIndex + 1  )
        {
            if( SaveAndLoadGameData.instance.savedData.unlockedLevels.Any(s => s.levelIndex == currentlyPlayingLevelIndex+2) == false )
            {
                
                SaveAndLoadGameData.instance.savedData.unlockedLevels.Add(new PlayerLevelData(){
                    levelIndex = currentlyPlayingLevelIndex + 2,
                    mark = 0
                });
                    
                SaveAndLoadGameData.instance.Save();
            }
        }
    }
    public void SaveCreatedLevel(ZenSceneData levelSceneData, LevelContentData LevelContent)
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
                levelContent = LevelContent,
                levelData = levelSceneData
            };
        }
        else
        {
            levelData = new LevelData()
            {
                levelIndex = 1,
                levelSceneIndex = -1,
                levelContent = LevelContent,
                levelData = levelSceneData
            };
        }       
        createdLevels.createdLevels.Add(levelData);
        levels = createdLevels.createdLevels;
    }
    public void DeleteLevel(int lvlIndex)
    {
        Debug.Log(lvlIndex);
        if( !createdLevels.createdLevels[lvlIndex].isMaster )
        {
            createdLevels.createdLevels.RemoveAt(lvlIndex);

            foreach (var item in createdLevels.createdLevels) // decrease index by 1
            {
                if( item.levelIndex >= lvlIndex + 1 )
                {
                    item.levelIndex -= 1;
                }
            }
            foreach (var item in  SaveAndLoadGameData.instance.savedData.unlockedLevels)
            {
                if( item.levelIndex >= lvlIndex + 1 )
                {
                    item.levelIndex -= 1;
                }
            }
        }
        else
        {
            Debug.Log("One of the levels you wnat to edit is master!");
        }
    }
    public void MoveLevelToLeft(int lvlIndex)
    {
        if( !createdLevels.createdLevels[lvlIndex].isMaster && !createdLevels.createdLevels[lvlIndex - 1].isMaster )
        {
            createdLevels.createdLevels.Swap( lvlIndex, lvlIndex - 1 );

            createdLevels.createdLevels[lvlIndex].levelIndex = lvlIndex + 1;
            createdLevels.createdLevels[lvlIndex - 1].levelIndex = lvlIndex;

            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex].levelIndex = lvlIndex + 1;

            SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex - 1].levelIndex = lvlIndex;
        }
        else
        {
            Debug.Log("One of the levels you want to edit is master!");
        }
    }
    public void MoveLevelToRight(int lvlIndex)
    {
        if( !createdLevels.createdLevels[lvlIndex].isMaster && lvlIndex + 1 < createdLevels.createdLevels.Count )
        {
            createdLevels.createdLevels.Swap( lvlIndex, lvlIndex + 1 );
            createdLevels.createdLevels[lvlIndex].levelIndex = lvlIndex + 1;
            createdLevels.createdLevels[lvlIndex + 1].levelIndex = lvlIndex + 2;

            
            SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex].levelIndex = lvlIndex + 1;
            
            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )    
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex + 1].levelIndex = lvlIndex + 2;
        }
        else
        {
            Debug.Log("One of the levels you wnat to edit is master or out of bound!");
        }
    }
}
