using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "New CreatedLevels", menuName = "Playground/CreatedLevels", order = 0)]
public class CreatedLevels : ScriptableObject 
{
    public List<LevelData> createdLevels = new List<LevelData>();    

    public void SaveCreatedLevel(ZenSceneData levelSceneData, LevelContentData LevelContent)
    {
        Debug.Log("level data saving... ");
        LevelData levelData;
        
        if(createdLevels.Count > 0)
        {
            LevelData lastLevel = createdLevels.Last();
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
        createdLevels.Add(levelData);

        #if UNITY_EDITOR // Only in Editor
        EditorUtility.SetDirty(this);
        #endif
    }

    public void DeleteLevel(int lvlIndex)
    {
        Debug.Log(lvlIndex);
        if( !createdLevels[lvlIndex].isMaster )
        {
            createdLevels.RemoveAt(lvlIndex);

            foreach (var item in createdLevels) // decrease index by 1
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
            #if UNITY_EDITOR // Only in Editor
            EditorUtility.SetDirty(this);
            #endif
        }
        else
        {
            Debug.Log("One of the levels you wnat to edit is master!");
        }
    }
    public void MoveLevelToLeft(int lvlIndex)
    {
        if( !createdLevels[lvlIndex].isMaster && !createdLevels[lvlIndex - 1].isMaster && lvlIndex - 1 >= 0 )
        {
            createdLevels.Swap( lvlIndex, lvlIndex - 1 );

            createdLevels[lvlIndex].levelIndex = lvlIndex + 1;
            createdLevels[lvlIndex - 1].levelIndex = lvlIndex;

            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex].levelIndex = lvlIndex + 1;
            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex - 1].levelIndex = lvlIndex;

            #if UNITY_EDITOR // Only in Editor
            EditorUtility.SetDirty(this);
            #endif
        }
        else
        {
            Debug.Log("One of the levels you want to edit is master!");
        }
    }
    public void MoveLevelToRight(int lvlIndex)
    {
        if( !createdLevels[lvlIndex].isMaster && lvlIndex + 1 < createdLevels.Count )
        {
            createdLevels.Swap( lvlIndex, lvlIndex + 1 );
            createdLevels[lvlIndex].levelIndex = lvlIndex + 1;
            createdLevels[lvlIndex + 1].levelIndex = lvlIndex + 2;

            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex].levelIndex = lvlIndex + 1;
            
            if( lvlIndex < SaveAndLoadGameData.instance.savedData.unlockedLevels.Count )    
                SaveAndLoadGameData.instance.savedData.unlockedLevels[lvlIndex + 1].levelIndex = lvlIndex + 2;
            
            #if UNITY_EDITOR // Only in Editor
            EditorUtility.SetDirty(this);
            #endif
        }
        else
        {
            Debug.Log("One of the levels you wnat to edit is master or out of bound!");
        }
    }
}
