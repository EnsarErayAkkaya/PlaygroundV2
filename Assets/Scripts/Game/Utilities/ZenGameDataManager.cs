using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenGameDataManager : MonoBehaviour
{
    void Awake()
    {
        RailManager railManager = FindObjectOfType<RailManager>();
        GameDataManager.instance.zenSceneDataManager.LoadZenSceneData();
        foreach (Rail item in railManager.GetRails())
        {
            railManager.ConnectCollidingRailPoints(item);
        }
    }
    

}
