﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RailData
{
    public GameObject railPrefab;
    public RailType railType;
    public Sprite railImage;
    public string name;
    public int cost;
    public bool isStart;
    public bool isEnd;
}
public enum RailType
{
    A, A2, A3, EL, ER, EEL, EER, F1, G2, H, L, NUp, NDown, P, T, G1, A1, F2
}
