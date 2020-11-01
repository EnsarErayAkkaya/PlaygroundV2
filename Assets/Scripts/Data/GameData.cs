﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{ 
    public PlaygroundType choosenPlayground;
    public List<RailType> playerRails;
    public List<EnvType> playerEnvs;
    public List<PlaygroundType> playerPlaygrounds;
    public List<TrainType> playerTrains;
    public int maxZenSceneSaveCount;
    public List<ZenSceneData> zenSceneDatas;
    public List<LevelData> unlockedLevels;
   
    public GameData()
    {
        playerRails = new List<RailType>();
        playerEnvs = new List<EnvType>();
        unlockedLevels = new List<LevelData>();
    }
    
}

