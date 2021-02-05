using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseData", menuName = "Playground/PurchaseData", order = 0)]
public class PurchaseData : ScriptableObject {
    public List<PurchaseItem> purchaseItems0;
    public List<PurchaseItem> purchaseItems;
}
[System.Serializable]
public class PurchaseItem
{
    public string name;
    public string title;
    public Sprite image;
    public bool withLeaf;
    public float cost;
    public bool isRail = true;
    public RailType railType;
    public bool isTrain;
    public TrainType trainType;
    public bool isEnv;
    public EnvType envType;
    public bool isCarpet;
    public PlaygroundType carpetType;
}