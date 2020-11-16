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
        levels = new List<LevelData>();
        levels.AddRange(createdLevels.createdLevels);
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
#if UNITY_EDITOR
    public void SaveCreatedLevel(ZenSceneData levelSceneData, LevelContentData LevelContent)
    {
        
            createdLevels.SaveCreatedLevel(levelSceneData, LevelContent);
        
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
    public void DeleteLevel(int lvlIndex)
    {
        
            createdLevels.DeleteLevel(lvlIndex);
       
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
    public void MoveLevelToLeft(int lvlIndex)
    {
        
            createdLevels.MoveLevelToLeft(lvlIndex);
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
    public void MoveLevelToRight(int lvlIndex)
    {
        
            createdLevels.MoveLevelToRight(lvlIndex);
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
#elif UNITY_ANDROID || UNITY_STANDALONE
    public void SaveCreatedLevel(ZenSceneData levelSceneData, LevelContentData LevelContent)
    {
        
            Debug.Log("level data saving... ");
            LevelData levelData;

            int lastLevelIndex = SaveAndLoadGameData.instance.savedData.playerCreatedLevels.Count + createdLevels.createdLevels.Count;
            
            levelData = new LevelData()
            {
                levelIndex = lastLevelIndex + 1,
                levelSceneIndex = -1,
                levelContent = LevelContent,
                levelData = levelSceneData
            };
                
            SaveAndLoadGameData.instance.savedData.playerCreatedLevels.Add(levelData);    
        
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
    public void DeleteLevel(int lvlIndex)
    {
        
        lvlIndex = (lvlIndex - createdLevels.createdLevels.Count);
            
            Debug.Log(lvlIndex);
            if( !SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex].isMaster )
            {
                SaveAndLoadGameData.instance.savedData.playerCreatedLevels.RemoveAt(lvlIndex);

                foreach (var item in SaveAndLoadGameData.instance.savedData.playerCreatedLevels) // decrease index by 1
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
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
    public void MoveLevelToLeft(int lvlIndex)
    {
        
            lvlIndex = (lvlIndex - createdLevels.createdLevels.Count);
            Debug.Log(lvlIndex);
            if( !SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex].isMaster 
            && !SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex - 1].isMaster && lvlIndex - 1 >= 0 )
            {
                SaveAndLoadGameData.instance.savedData.playerCreatedLevels.Swap( lvlIndex, lvlIndex - 1 );

                SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex].levelIndex = lvlIndex + 1;
                SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex - 1].levelIndex = lvlIndex;

                if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                    SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex].levelIndex = lvlIndex + 1;
                if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                    SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex - 1].levelIndex = lvlIndex;
            }
            else
            {
                Debug.Log("One of the levels you want to edit is master!");
            }
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
    public void MoveLevelToRight(int lvlIndex)
    {
        
            lvlIndex = (lvlIndex - createdLevels.createdLevels.Count);
            Debug.Log(lvlIndex);
            if( !SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex].isMaster 
            && lvlIndex + 1 < SaveAndLoadGameData.instance.savedData.playerCreatedLevels.Count )
        {
            SaveAndLoadGameData.instance.savedData.playerCreatedLevels.Swap( lvlIndex, lvlIndex + 1 );
            SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex].levelIndex = lvlIndex + 1;
            SaveAndLoadGameData.instance.savedData.playerCreatedLevels[lvlIndex + 1].levelIndex = lvlIndex + 2;

            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex].levelIndex = lvlIndex + 1;
            
            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )    
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex + 1].levelIndex = lvlIndex + 2;
        }
        else
        {
            Debug.Log("One of the levels you wnat to edit is master or out of bound!");
        }
        
        SaveAndLoadGameData.instance.Save();
        GetLevels();
    }
#endif
    public void GetLevels()
    {
        levels.Clear();
        levels.AddRange(createdLevels.createdLevels);
        if(SaveAndLoadGameData.instance.savedData.playerCreatedLevels != null && SaveAndLoadGameData.instance.savedData.playerCreatedLevels.Count > 0)
            levels.AddRange(SaveAndLoadGameData.instance.savedData.playerCreatedLevels);
    }
}
