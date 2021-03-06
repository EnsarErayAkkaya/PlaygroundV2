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
    public List<PlayerLevelData> unlockedLevels;
    public List<LevelData> playerCreatedLevels;
    public bool musicON = true;
    public bool soundON = true;
    public int interactibleContentValue = 3;
    public int leaf = 0;
    public bool postProccessing = true;
    public GameData()
    {
        playerRails = new List<RailType>();
        playerEnvs = new List<EnvType>();
        unlockedLevels = new List<PlayerLevelData>();
        playerCreatedLevels = new List<LevelData>();
        interactibleContentValue = 3;
}
    
}

