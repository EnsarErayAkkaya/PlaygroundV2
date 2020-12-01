using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelContentData 
{
    public string levelName;
    public int budget;
    public List<RailType> levelRails;
    public List<EnvType> levelEnvs;
    public List<TrainType> levelTrains;
    public List<RailType> rewardRails;
    public List<EnvType> rewardEnvs;
    public List<TrainType> rewardTrains;
}
