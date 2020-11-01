﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZenSceneDataManager : MonoBehaviour
{
    [SerializeField] GameDataManager dataManager;
    
    public ZenSceneData LoadingScene = null;
    public bool isLoad = false;
   
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
            }
            railManager.nextIndex = railManager.GetRails().Last().index + 1;

            foreach (var item in LoadingScene.envsData)
            {
                Instantiate(dataManager.allEnvs.First(e => e.envType == item.envType).envPrefab, item.position, Quaternion.Euler(item.rotation)).GetComponent<EnvironmentObject>();
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
}
