using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class TrainData 
{
    public GameObject trainPrefab;
    public Sprite trainImage;
    public string name;
    public TrainType trainType;
    public int cost;
}  

public enum TrainType
{
    A, AA
}