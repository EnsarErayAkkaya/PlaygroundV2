using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZenSceneDataManager : MonoBehaviour
{
    [SerializeField] GameDataManager dataManager;
    
    public ZenSceneData LoadingScene = null;
    public bool isLoad = false;
    public bool isLevel = false;
    public int levelIndex;
   
    public void SaveZenSceneData( string imagePath)
    {
        ZenSceneData data = new ZenSceneData();

        data.imagePath = imagePath;
        
        data.dateTime = System.DateTime.Today;

        // save rails
        foreach ( var item in FindObjectOfType<RailManager>().GetRails() )
        {
            data.railsData.Add(new RailSaveData(){
                scale = item.transform.localScale,
                position = item.transform.position,
                rotation = item.transform.rotation.eulerAngles,
                railType = item.railType,
                id = item.index
            });
        }
        // save envs
        foreach ( var item in FindObjectOfType<EnvironmentManager>().GetEnvironments() )
        {
            data.envsData.Add(new EnvSaveData(){
                position = item.transform.position,
                rotation = item.transform.rotation.eulerAngles,
                envType = item.envType,
                id = 0
            });
        }
        // save trains
        foreach ( var item in FindObjectOfType<TrainManager>().GetTrains() )
        {
            data.trainsData.Add(new TrainSaveData(){
                position = item.transform.parent.transform.position,
                rotation = item.transform.parent.rotation.eulerAngles,
                trainType = item.trainType,
                id = 0,
                startingRailId = item.startingRailId
            });
        }
        // save playground
        PlayGround pl = FindObjectOfType<PlaygroundManager>().playground;
        data.playgroundData = new PlaygroundSaveData()
        {
            position = pl.transform.position,
            rotation = pl.transform.rotation.eulerAngles,
            playgroundType = pl.playgroundType,
            id = 0
        };

        SaveData(data);
    }
    public void SaveLevelSceneData(LevelContentData levelContentData, int index = -1)
    {
        ZenSceneData data = new ZenSceneData();

        data.imagePath = "";
        
        data.dateTime = System.DateTime.Today;

        // save rails
        foreach ( var item in FindObjectOfType<RailManager>().GetRails() )
        {
            data.railsData.Add(new RailSaveData(){
                isStatic = item.isStatic,
                scale = item.transform.localScale,
                position = item.transform.position,
                rotation = item.transform.rotation.eulerAngles,
                railType = item.railType,
                id = item.index,
                isStart = item.isStart,
                isEnd = item.isEnd
            });
        }
        // save envs
        foreach ( var item in FindObjectOfType<EnvironmentManager>().GetEnvironments() )
        {
            data.envsData.Add(new EnvSaveData(){
                isStatic = true,
                position = item.transform.position,
                rotation = item.transform.rotation.eulerAngles,
                envType = item.envType,
                id = 0
            });
        }
        // save trains
        foreach ( var item in FindObjectOfType<TrainManager>().GetTrains() )
        {
            data.trainsData.Add(new TrainSaveData(){
                isStatic = false,
                position = item.transform.parent.transform.position,
                rotation = item.transform.parent.rotation.eulerAngles,
                trainType = item.trainType,
                id = 0,
                startingRailId = item.startingRailId
            });
        }

        // save playground
        PlayGround pl = FindObjectOfType<PlaygroundManager>().playground;
        data.playgroundData = new PlaygroundSaveData()
        {
            isStatic = true,
            position = pl.transform.position,
            rotation = pl.transform.rotation.eulerAngles,
            playgroundType = pl.playgroundType,
            id = 0
        };
        
        foreach ( var item in FindObjectsOfType<TrainCollectable>() )
        {
            data.collectableData.Add(new CollectableSaveData(){
                isStatic = false,
                position = item.transform.transform.position,
                rotation = item.transform.rotation.eulerAngles,
                id = 0
            });
        }
        if (index != -1)
            ReplaceLevelData(data, levelContentData, index);
        else
            SaveLevelData(data, levelContentData);
    }
    public void LoadZenSceneData()
    {
        PlaygroundManager playgroundManager = FindObjectOfType<PlaygroundManager>();
        RailManager railManager = FindObjectOfType<RailManager>(); 
        if(isLoad == true)
        {
            playgroundManager.playground = Instantiate( dataManager.allPlaygrounds.First( p => p.playgroundType == LoadingScene.playgroundData.playgroundType).playgroundGamePrefab).GetComponent<PlayGround>();
            
            foreach (var item in LoadingScene.railsData)
            {
                Rail r = Instantiate(dataManager.allRails.First(s => s.railType == item.railType).railPrefab, item.position, Quaternion.Euler(item.rotation)).GetComponent<Rail>();
                r.transform.localScale = item.scale;
                railManager.AddRail( r,  item.id );
                r.isStart = item.isStart;
                r.isEnd = item.isEnd;
                if (isLevel)
                    r.isStatic = false;
                else
                    r.isStatic = item.isStatic;
            }
            railManager.nextIndex = railManager.GetRails().Last().index + 1;

            foreach (var item in LoadingScene.envsData)
            {
                EnvironmentObject env = Instantiate(dataManager.allEnvs.First(e => e.envType == item.envType).envPrefab, item.position, Quaternion.Euler(item.rotation)).GetComponent<EnvironmentObject>();
                if (isLevel)
                    env.isStatic = false;
                else
                    env.isStatic = item.isStatic;            
            }            
            Rail[] rails = FindObjectsOfType<Rail>();
            foreach (var item in LoadingScene.trainsData)
            {
                Train t = Instantiate(dataManager.allTrains.First(s => s.trainType == item.trainType).trainPrefab,
                 item.position,
                  Quaternion.Euler(item.rotation))
                  .transform.GetChild(0).GetComponent<Train>();
                
                if( rails.Length < 1 )
                {
                    rails = FindObjectsOfType<Rail>();
                    Debug.Log(rails.Length);
                }
                t.startingRailId = item.startingRailId;
                t.rail = rails.FirstOrDefault( f => f.index == item.startingRailId );
                if (isLevel)
                    t.isStatic = false;
                else
                    t.isStatic = item.isStatic;
            }
            CollectableManager collectableManager = FindObjectOfType<CollectableManager>(); 
            foreach (var item in LoadingScene.collectableData)
            {
                TrainCollectable c = Instantiate( dataManager.collectablePrefab , item.position, Quaternion.Euler(item.rotation) ).GetComponent<TrainCollectable>();
                collectableManager.Add(c);
                if (isLevel)
                    c.isStatic = false;
                else
                    c.isStatic = item.isStatic;            
            }            
        }   
    }
    void SaveData(ZenSceneData data)
    {
        // Eğer  max kayıt sayısı > kayıtlı sahne sayısından
        if(SaveAndLoadGameData.instance.savedData.maxZenSceneSaveCount > SaveAndLoadGameData.instance.savedData.zenSceneDatas.Count )
        {
            SaveAndLoadGameData.instance.savedData.zenSceneDatas.Add(data);
        }
        // eğer  max kayıt sayısı <= kayıtlı sahne sayısından (küçük olamazda işte, anla sen)
        else
        {
            SaveAndLoadGameData.instance.savedData.zenSceneDatas.RemoveAt(0); // en eskisini sil
            SaveAndLoadGameData.instance.savedData.zenSceneDatas.Add(data);
        }
        SaveAndLoadGameData.instance.Save();
    }
    void SaveLevelData(ZenSceneData data, LevelContentData levelContentData)
    {
        dataManager.SaveCreatedLevel(data, levelContentData);
    }
    void ReplaceLevelData(ZenSceneData data, LevelContentData levelContentData, int index)
    {
        dataManager.ReplaceLevel(data, levelContentData, index);
    }
}
